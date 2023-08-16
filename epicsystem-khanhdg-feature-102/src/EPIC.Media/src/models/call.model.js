const mongoose = require('mongoose');
const { toJSON, paginate } = require('./plugins');

const callSchema = new mongoose.Schema({
    caller: {
        type: Object,
        required: true,
    },
    receiver: {
       type: Object,
       required: true,
    },
    status: {
        enum: ['SUCCESS','MISSING'],
        default: 'SUCCESS',
        type: String,
        required: true,
    },
    urlAudio: {
        type: String,
    },
    startTime: {
        type: Date,
        required: true,
    },
    endTime: {
        type: Date,
    },
    duration: {
        type: String,
    },
});

// Add plugin that converts mongoose documents to JSON
callSchema.plugin(toJSON);
// Add plugin for pagination
callSchema.plugin(paginate);

// Create model from the schema
const Call = mongoose.model('Call', callSchema);

module.exports = Call;
