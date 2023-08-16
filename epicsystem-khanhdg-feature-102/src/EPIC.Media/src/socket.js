const { Server } = require('socket.io');
const { createAdapter } = require('@socket.io/redis-adapter');
const { createClient } = require('redis');
const { redisUrl } = require('./config/config');

let IO;
const roomData = {};
const callTimeouts = {};
const activeCalls = new Map();

module.exports.initIO = (httpServer) => {
  IO = new Server(httpServer, {
    path: '/api/media/socket',
  });

  const pubClient = createClient({ url: redisUrl });
  const subClient = pubClient.duplicate();

  Promise.all([pubClient.connect(), subClient.connect()]).then(() => {
    IO.adapter(createAdapter(pubClient, subClient));
    IO.listen(4000);
  });

  IO.use((socket, next) => {
    if (socket.handshake.query) {
      const { userInfo } = socket.handshake.query;
      socket.info = JSON.parse(userInfo);
      const { sub, phone } = socket.info;
      if (sub) {
        socket.user = sub;
      } else {
        socket.user = phone;
      }
      next();
    }
  });

  IO.on('connection', (socket) => {
    console.log(socket.user, 'Connected');
    socket.join(socket.user);

    const tradingProviderId = socket.info.trading_provider_id;
    if (tradingProviderId) {
      roomData[tradingProviderId] = roomData[tradingProviderId] || [];
      roomData[tradingProviderId].push(socket.user);
    }

    socket.on('online', (trading) => {
      const onlines = roomData[trading].filter((id) => !activeCalls.has(id));
      socket.emit('canCall', onlines);
    });

    socket.on('signalCall', (otherId) => {
      const isBusy = activeCalls.has(otherId);
      if (isBusy) {
        socket.emit('busy', { message: 'Đường dây bận' });
      } else {
        socket.to(otherId).emit('signalCall', {
          callerInfo: socket.info,
          callerId: socket.user,
        });
      }
      const timeout = setTimeout(() => {
        const timeoutMessage = 'Không có ai trả lời!';
        socket.emit('timeout', { message: timeoutMessage });
        socket.to(otherId).emit('timeout', { message: timeoutMessage });
        delete callTimeouts[socket.user];
        delete callTimeouts[otherId];
      }, 30000);
      callTimeouts[socket.user] = timeout;
      callTimeouts[otherId] = timeout;
    });

    socket.on('accept', (otherId) => {
      socket.to(otherId).emit('accept', {
        accept: true,
        calleeInfo: socket.info,
      });
    });

    socket.on('call', (data) => {
      const calleeId = data.calleeId;

      socket.to(calleeId).emit('newCall', {
        callerId: socket.user,
        payload: data.payload,
      });
    });

    socket.on('answer', (data) => {
      const callerId = data.callerId;
      socket.to(callerId).emit('answered', {
        calleeId: socket.user,
        payload: data.payload,
      });
      clearTimeout(callTimeouts[callerId]);
      clearTimeout(callTimeouts[socket.user]);
      delete callTimeouts[callerId];
      delete callTimeouts[socket.user];
      activeCalls.set(socket.user, callerId);
    });

    socket.on('video', (data) => {
      socket.to(data.otherId).emit('enableVideo', {
        video: data.video,
      });
    });

    socket.on('rejectCall', (otherId) => {
      socket.to(otherId).emit('callRejected', {
        message: 'Cuộc gọi bị từ chối',
      });
    });

    socket.on('endCall', (otherId) => {
      socket.to(otherId).emit('callEnded', { message: 'Cuộc gọi kết thúc' });
      activeCalls.delete(socket.user);
      activeCalls.delete(otherId);
    });

    socket.on('candidate', (data) => {
      const calleeId = data.calleeId;
      socket.to(calleeId).emit('candidate', {
        sender: socket.user,
        payload: data.payload,
      });
    });

    socket.on('disconnect', () => {
      console.log(socket.user, 'Disconnected');
      const room = activeCalls.get(socket.user);

      if (room) {
        const otherId = room !== socket.user ? room : null;
        if (otherId) {
          socket.to(otherId).emit('callEnded', { message: 'Cuộc gọi kết thúc' });
        }
        activeCalls.delete(socket.user);
      }

      if (roomData[tradingProviderId]) {
        roomData[tradingProviderId] = roomData[tradingProviderId].filter((id) => id !== socket.user);
        if (roomData[tradingProviderId].length === 0) {
          delete roomData[tradingProviderId];
        }
      }
    });
  });
};

module.exports.getIO = () => {
  if (!IO) {
    throw Error('IO not initialized.');
  } else {
    return IO;
  }
};
