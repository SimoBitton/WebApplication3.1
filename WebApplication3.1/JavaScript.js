function circle(lon, lat, myCanvas) {
    myCanvas.lineWidth = '3';
    myCanvas.strokeStyle = "navy";
    myCanvas.beginPath();
    myCanvas.arc(lon, lat, 5, 0, 2 * Math.PI);
    myCanvas.fillStyle = "red";
    myCanvas.fill();
    myCanvas.stroke();
}


function drawPath(startLon, startLat, allLocations, myCanvas) {
    myCanvas.beginPath();
    myCanvas.moveTo(startLon, startLat);
    for (var i = 0; i < allLocations.length; i++) {
        myCanvas.lineTo(allLocations[i].x, allLocations[i].y);
        myCanvas.strokeStyle = "red";
        myCanvas.stroke();
    }
}