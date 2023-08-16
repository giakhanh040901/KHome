import { AfterViewInit, Component, ElementRef, Injector, OnInit, ViewChild, Renderer2 } from '@angular/core';
import { AppConsts, CallSupportConst, TypeFormatDateConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ContractTemplateServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { CallSupportService } from '@shared/services/call-support.service';
import { SignalingService } from '@shared/services/signaling.service';
import { Socket } from 'ngx-socket-io';
import { MessageService } from 'primeng/api';
import * as moment from "moment";

@Component({
  selector: 'app-call-support',
  templateUrl: './call-support.component.html',
  styleUrls: ['./call-support.component.scss']
})
export class CallSupportComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private renderer: Renderer2,
    private signalingService: SignalingService,
    private socket: Socket,
    private _callSupportService: CallSupportService,
    private _contractTemplateService: ContractTemplateServiceProxy,

  ) {
    super(injector, messageService);
    this.userLogin = this.getUser();
  }
  userLogin: any = {};
  @ViewChild('localVideo') localVideo: ElementRef;
  @ViewChild('remoteVideo') remoteVideo: ElementRef;
  @ViewChild('audioPlayer') audioPlayer: ElementRef;
  @ViewChild('rowAudioPlayer') rowAudioPlayer: ElementRef;
  facetime: boolean = false;
  otherUserVideo: boolean = false
  otherUserRequest: boolean = false;
  sdp: any;
  urlAudio: string;
  candidate: any[] = [];
  isReceiveCandidate: boolean = false
  otherId: any;
  record: boolean = false;
  configuration: RTCConfiguration = {
    iceServers: [
      {
        urls: ['turn:turn-dev.epicpartner.vn:3478?transport=tcp'],
        username: 'ste_stun',
        credential: 'admin123',
      },
    ],
  };
  localStream: MediaStream | null;
  isLocalCameraOn: boolean = false;
  remotestream: MediaStream | null
  startTime: Date;
  timeCalling: Date;
  dataFilter: any;
  rows: any[] = [];
  cols: any[];
  _selectedColumns: any[];
  AppConsts = AppConsts;
  CallSupportConst = CallSupportConst;
  connection: RTCPeerConnection;
  caller: any;
  currentTime: string;
  timer: any;
  statusCall: any = {
    incomingCall: true,
    progressingCall: false,
    endedCall: false
  };
  statusCallEnded: string;
  private mediaRecorder: MediaRecorder | null = null;
  private chunks: Blob[] = [];
  ngOnInit(): void {
    this.socket.on('connect', () => {
      console.log('Kết nối Socket.IO thành công!');
    });
    this.setPage();
    this.currentTime = '00:00';
    this.cols = [
      { field: 'callerName', header: 'Tên người gọi', width: '18rem', class: 'justify-content-left', isPin: true },
      { field: 'callerPhone', header: 'Số điện thoại người gọi', width: '18rem', class: 'justify-content-left', isPin: true, isResize: true },
      // { field: 'phone', header: 'Email người gọi', class: 'justify-content-left', width: '10rem', isPin: true },
      { field: 'receiverName', header: 'Tên người nhận', class: 'justify-content-left', width: '18rem' },
      // { field: 'dateOfBirth', header: 'Email người nhận', class: 'justify-content-left', width: '12rem' },
      { field: 'startTime', header: 'Thời gian bắt đầu cuộc gọi', width: '18rem', class: 'justify-content-left', cutText: 'b-cut-text-18' },
      { field: 'endTime', header: 'Thời gian kết thúc cuộc gọi', width: '18rem', class: 'justify-content-center', pTooltip: 'Nhà đầu tư chuyên nghiệp', tooltipPosition: 'top' },
      { field: 'duration', header: 'Thời gian cuộc gọi', width: '18rem', class: 'justify-content-center' },
      { field: 'urlAudio', header: 'Cuộc gọi', width: '28rem', class: 'justify-content-center' },
    ];

    this.cols = this.cols.map((item, index) => {
      item.position = index + 1;
      return item;
    })
    this.initializeConnection();
    this._selectedColumns = this.cols;

  }

  setStatus(key: string) {
    Object.keys(this.statusCall).forEach((k) => {
      this.statusCall[k] = (k === key);
    });
  }

  playAudio() {
    const audio: HTMLAudioElement = this.audioPlayer.nativeElement;
    audio.play();
  }

  pauseAudio() {
    const audio: HTMLAudioElement = this.audioPlayer.nativeElement;
    audio.pause();
  }

  private async _initConnection(): Promise<void> {
    this.connection = new RTCPeerConnection(this.configuration);
    this._registerConnectionListeners();
    await this._getStreams()
  }

  public async makeCall(): Promise<void> {
    this.renderer.addClass(document.querySelector('.btn-init-call'), 'shake');

    setTimeout(() => {
      this.renderer.removeClass(document.querySelector('.btn-init-call'), 'shake');
    }, 5000);
    this.connection.createOffer({ offerToReceiveVideo: true }).then(sdp => {
      this.connection.setLocalDescription(sdp);
      this.signalingService.sendMessage('call', {
        calleeId: this.otherId,
        payload: sdp
      })
    });
  }

  public async createAnswer(): Promise<void> {
    this.stopTimer();
    this.startTimer();
    this.startTime = new Date();
    this.setStatus('progressingCall');
    this.pauseAudio();
    if (this.connection) {
      this.connection.createAnswer({ offerToReceiveVideo: 1 }).then(sdp => {
        this.connection.setLocalDescription(sdp)
        this.signalingService.sendMessage('answer', {
          callerId: this.otherId,
          payload: sdp
        })
      })
    }
  }

  public async acceptCall(): Promise<void> {
    this.signalingService.sendMessage('accept', this.otherId)
  }

  public async endCall(status: string): Promise<void> {
    if (status === 'rejectCall') {
      let body = {
        caller: this.caller,
        receiver: this.userLogin,
        status: 'MISSING',
        startTime: this.timeCalling,
      }
      this.create(body);
    } else if (status === 'endCall') {
      this.statusCallEnded = 'SUCCESS'
      this.isReceiveCandidate = false
      this.mediaRecorder.stop();
    }
    this.signalingService.sendMessage(status, this.otherId)
    this.otherId = null
    this.sdp = null
    this.candidate = []
    this.setStatus('endedCall');
    this.pauseAudio();
  }

  private _registerConnectionListeners(): void {
    this.connection.onicecandidate = event => {
      if (event.candidate) {
        this.signalingService.sendMessage('candidate', {
          payload: event.candidate,
          calleeId: this.otherId,
        }
        )
      }
    }
  }

  public toggleLocalCamera(): void {
    if (!this.facetime) {
      this.facetime = true;
      this.isLocalCameraOn = true;
      this.signalingService.sendMessage('video', {
        video: this.isLocalCameraOn,
        otherId: this.otherId,
      })
    } else {
      this.localStream.getVideoTracks().forEach((track) => {
        track.enabled = !this.isLocalCameraOn;
      });
      this.isLocalCameraOn = !this.isLocalCameraOn;
      this.signalingService.sendMessage('video', {
        video: this.isLocalCameraOn,
        otherId: this.otherId,
      })
    }
  }

  private async _getStreams(): Promise<void> {
    this.localStream = await navigator.mediaDevices.getUserMedia({
      video: false,
      audio: true,
    });

    this.localVideo.nativeElement.srcObject = this.localStream;

    this.localStream.getTracks().forEach((track) => {
      this.connection.addTrack(track, this.localStream);
    });

    this.connection.ontrack = (event) => {
      this.remotestream = event.streams[0]
      event.streams.forEach((stream) => {
        this.remoteVideo.nativeElement.srcObject = stream;
      });
      this.handleRecord()
    };

  }

  handleRecord() {
    var OutgoingAudioMediaStream = new MediaStream();
    OutgoingAudioMediaStream.addTrack(this.localStream?.getAudioTracks()[0]);

    var IncomingAudioMediaStream = new MediaStream();
    IncomingAudioMediaStream.addTrack(this.remotestream?.getAudioTracks()[0]);

    const audioContext = new AudioContext();

    var audioCMS = audioContext.createMediaStreamSource(OutgoingAudioMediaStream);
    var audioApp = audioContext.createMediaStreamSource(IncomingAudioMediaStream);

    var dest = audioContext.createMediaStreamDestination();

    audioCMS.connect(dest);
    audioApp.connect(dest);

    dest.stream.addTrack(this.localStream.getAudioTracks()[0]);
    var recordingStream = dest.stream;

    this.mediaRecorder = new MediaRecorder(recordingStream)

    // Xử lý sự kiện khi có dữ liệu âm thanh mới từ MediaRecorder
    this.mediaRecorder.addEventListener('dataavailable', event => {
      this.chunks.push(event.data);
    });

    this.mediaRecorder.addEventListener('stop', () => {
      const audioBlob = new Blob(this.chunks, { type: 'audio/wav' });
      const file = new File([audioBlob], `record_${moment().format('HH-mm YYYY-MM-DD')}.mp3`, { type: 'audio/mp3' });
      let folder = `folder-call-support/${moment().format('MM-DD-YYYY')}`
      this._contractTemplateService.uploadFileGetUrl(file, folder).subscribe((response) => {
        this.urlAudio = response.data;
        this.facetime = false;
        this.chunks = [];
        let body = {
          caller: this.caller,
          receiver: this.userLogin,
          status: this.statusCallEnded,
          urlAudio: this.urlAudio,
          startTime: this.startTime,
          endTime: this.calculateEndTime(),
          duration: this.currentTime,
        }
        this.create(body);
      })
    });
    this.mediaRecorder.start()
  }

  calculateEndTime() {
    const [minutes, seconds] = this.currentTime.split(':');

    const minutesToAdd = parseInt(minutes, 10);
    const secondsToAdd = parseInt(seconds, 10);

    const endTime = new Date(this.startTime);

    endTime.setMinutes(endTime.getMinutes() + minutesToAdd);
    endTime.setSeconds(endTime.getSeconds() + secondsToAdd);
    return endTime
  }

  create(data: any) {
    this._callSupportService.create(data).subscribe((res) => {
      this.stopTimer();
      setTimeout(() => {
        this.setPage();
      }, 3000);
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
    });
  }

  setPage(pageInfo?: any) {

    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.keyword;
    this.isLoading = true;

    this._callSupportService.getAll(this.page, this.dataFilter).subscribe((res) => {
      this.isLoading = false;
      this.page.totalItems = res.data.totalResults;
      this.rows = res.data?.results;
      this.showData(this.rows);
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
    });
  }


  showData(rows) {
    for (let row of rows) {
      row.callerName = row?.caller?.name;
      row.callerPhone = row?.caller?.phone;
      row.receiverName = row?.receiver?.display_name;
      row.startTime = this.formatDate(row?.startTime, TypeFormatDateConst.DMYHms);
      row.endTime = row?.endTime ? this.formatDate(row?.endTime, TypeFormatDateConst.DMYHms) : null;

    };
  }

  async initializeConnection() {


    try {
      await this._initConnection();

      const messageTypes = [
        'newCall',
        'answered',
        'candidate',
        'calling',
        'timeout',
        'callRejected',
        'callEnded',
        'enableVideo',
        'signalCall'
      ];

      messageTypes.forEach((type) => {

        this.signalingService.getMessages(type).subscribe((payload) => {
          this._handleMessage(payload, type);
        }
        );

      });

    } catch (err) {
      console.log('err', err);
    }
  }

  seconds: number;
  startTimer() {
    clearInterval(this.timer); // Xóa đợt đếm trước đó nếu có

    this.seconds = 0;
    this.timer = setInterval(() => {
      const minutes = Math.floor(this.seconds / 60);
      const seconds = this.seconds % 60;
      this.currentTime = this.formatTime(minutes) + ':' + this.formatTime(seconds);
      this.seconds++;
    }, 1000);
  }

  stopTimer() {
    clearInterval(this.timer);
    this.currentTime = '00:00';
    this.seconds = 0;
  }

  formatTime(time: number) {
    return (time < 10) ? '0' + time : time.toString();
  }

  private async _handleMessage(data, action): Promise<void> {
    console.log('data', data, 'action-', action);
    switch (action) {
      case 'newCall':
        this.sdp = JSON.stringify(data.payload)
        this.connection.setRemoteDescription(new RTCSessionDescription(data.payload))
        break;
      case 'signalCall':
        this.timeCalling = new Date()
        this.startTimer();
        this.caller = data?.callerInfo;
        this.otherId = data.callerId;
        this.setStatus('incomingCall');
        this.playAudio();
        break
      case 'answered':
        this.otherId = data.calleeId
        this.sdp = JSON.stringify(data.payload)
        this.connection.setRemoteDescription(new RTCSessionDescription(data.payload))
        break;
      case 'candidate':
        if (data) {
          if (!this.isReceiveCandidate) {
            this.createAnswer()
            this.isReceiveCandidate = true
          }
          if (this.connection) {
            this.connection.addIceCandidate(new RTCIceCandidate(data.payload));
          } else {
            this.candidate.push(data.payload)
          }
        }
        break;
      case 'callRejected':
        this.statusCallEnded = 'MISSING'
        this.otherId = null
        this.sdp = null
        this.candidate = []
        break
      case 'callEnded':
        this.mediaRecorder.stop();
        this.statusCallEnded = 'SUCCESS'
        this.otherId = null
        this.sdp = null
        this.candidate = []
        this.isReceiveCandidate = false
        break
      // case 'enableVideo':
      //   if (!this.facetime) {
      //     this.otherUserRequest = true
      //   }
      //   this.otherUserVideo = data.video
      //   break
      case 'timeout':
        this.statusCallEnded = 'MISSING'
        this.pauseAudio();
        this.otherId = null
        let body = {
          caller: this.caller,
          receiver: this.userLogin,
          status: "MISSING",
          startTime: this.timeCalling,
        }
        this.create(body);
        break
      default:
        break;
    }
  }
}
