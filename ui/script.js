var map = L.map('map', {
    layers: MQ.mapLayer(),
    center: [ 51.5405, 46.0086 ],
    zoom: 13
});

MQ.trafficLayer().addTo(map);

L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png').addTo(map);

map.on('click', function(e) {
    onMapClick(e);
});