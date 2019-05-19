// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
navigator.mediaDevices.getUserMedia({audio:true})
    .then(stream => {handlerFunction(stream)})


function handlerFunction(stream) {
    rec = new MediaRecorder(stream);
    rec.ondataavailable = e => {
        audioChunks.push(e.data);
        if (rec.state == "inactive"){
            let blob = new Blob(audioChunks,{type:'audio/mpeg-3'});
            recordedAudio.src = URL.createObjectURL(blob);
            recordedAudio.controls=true;
            recordedAudio.autoplay=true;
            sendData(blob)
        }
    }
}
function sendData(soundBlob) {
    var fd = new FormData();
    fd.append('fname', 'test.mp3');
    fd.append('data', soundBlob);
    $.ajax({
        type: 'POST',
        url: '/upload',
        data: fd,
        processData: false,
        contentType: false
    }).done(function(data) {
        console.log(data);
    });
}

var recording = false;
record.onclick = e => {
    if(recording){
        recording = false;
        record.style.backgroundColor = "red"
        rec.stop();
    }
    else {
        recording = true;
        record.style.backgroundColor = "blue"
        audioChunks = [];
        rec.start();
    }
}