window.customElements.define("comp-google-map",
    class CompGoogleMap extends HTMLElement {
        //class propertileri
        selector = "[name=divGoogleMap]";
        map;
        drawingManager;
        polygons = [];
        markers = [];

        constructor() {
            super();

            this.innerHTML = `<div name="divGoogleMap" style="height:100%;"></div>`;
            this.fnInit();
        }

        connectedCallback() {
            //console.log("connectedCallback");
        }

        static get observedAttributes() {
            return ['drawing-manager', 'map-type-id', 'zoom', 'center', 'height'];
        }

        attributeChangedCallback(property, oldValue, newValue) {
            //console.log("attributeChangedCallback:", property, oldValue, newValue);
            if (oldValue === newValue) return;

            switch (property) {
                case "drawing-manager":
                    this.fnSetDrawingManagerOptions(JSON.parse(newValue));
                    break;
                case "map-type-id":
                    this.map.setMapTypeId(String(newValue));
                    break;
                case "zoom":
                    this.map.setZoom(parseInt(newValue));
                    break;
                case "center":
                    this.map.setCenter(JSON.parse(newValue));
                    break;
                case "height":
                    this.querySelector(this.selector).style.height = newValue + "px";
                    break;
            }
        }

        fnInit() {
            //map taným
            this.map = new google.maps.Map(this.querySelector(this.selector), {
                //mapTypeId: google.maps.MapTypeId.ROADMAP,
                //center: new google.maps.LatLng(39.9187725, 32.85806),
                //zoom: 11,
            });

            this.drawingManager = new google.maps.drawing.DrawingManager({
                //drawingMode: google.maps.drawing.OverlayType.MARKER, //açýldýðýnda hangisi seçilsin istersen
                drawingControl: false, //ilk açýldýðýnda yok olsun
                drawingControlOptions: {
                    position: google.maps.ControlPosition.TOP_CENTER,
                    drawingModes: [''] // [google.maps.drawing.OverlayType.MARKER, google.maps.drawing.OverlayType.POLYGON],
                },
                markerOptions: {
                    draggable: true
                },
                polygonOptions: {
                    draggable: true,
                    editable: true
                }
            });

            this.drawingManager.setMap(this.map);

            google.maps.event.addListener(this.drawingManager, 'overlaycomplete', (event) => {
                //console.log("overlaycomplete", event);
                //var coordinatesArray = event.overlay.getPath().getArray();

                if (event.type == "marker") {
                    this.fnAddMarker({ 'marker': event.overlay });
                }

                if (event.type == "polygon") {
                    this.fnAddPolygon({ 'polygon': event.overlay });
                }

                //sonda olacak
                this.dispatchEvent(new CustomEvent('overlaycomplete', { detail: { value: event } }));
            });

        }

        fnSetDrawingManagerOptions(overlayTypes) {
            //console.log(overlayTypes);
            this.drawingManager.setOptions({
                drawingControl: true,
                drawingControlOptions: {
                    position: google.maps.ControlPosition.TOP_CENTER,
                    drawingModes: overlayTypes
                },
                markerOptions: {
                    draggable: true
                }
            });

        }

        fnAddPolygon(prms) {
            //console.log("fnAddPolygon(prms):", prms);
            let polygon = null;
            if (prms.polygon == undefined) {
                //polygon = new google.maps.Polygon({
                //    editable: prms.editable,
                //    draggable: prms.draggable,
                //    paths: prms.paths,
                //    //strokeColor: "#FF0000",
                //    //strokeOpacity: 0.8,
                //    //strokeWeight: 2,
                //    //fillColor: "#FF0000",
                //    //fillOpacity: 0.35,
                //    map: this.map
                //});
                polygon = new google.maps.Polygon(prms);
                polygon.setMap(this.map);

            } else {
                polygon = prms.polygon;
            }

            var paths = polygon.getPaths();
            
            paths.forEach((path) => {
                path.addListener('set_at', (event) => {
                    this.dispatchEvent(new CustomEvent('polygonSet_at', { detail: { value: event } }));
                });
            });

            this.polygons.push(polygon);
        }

        fnDeleteAllPolygons() {
            if (this.polygons) {
                for (let i in this.polygons) {
                    this.polygons[i].setMap(null);
                }
                this.polygons = [];
            }
        }

        fnAddMarker(prms) {
            //console.log("fnAddMarker(prms):", prms);
            let marker = null;
            if (prms.marker == undefined) {
                //marker = new google.maps.Marker({
                //    //animation: google.maps.Animation.BOUNCE, //bisiklet kullanýmda ise kullanýcaz
                //    position: prms.position,
                //    title: prms.title,
                //    draggable: prms.draggable,
                //    icon: prms.icon,
                //    //map: this.map
                //});
                if (prms.icon) { prms.icon = prms.icon + "?p=" + new Date().getTime().toString() }
                marker = new google.maps.Marker(prms);
                marker.setMap(this.map);

            } else {
                marker = prms.marker;
            }

            marker.addListener("dragend", (event) => {
                //console.log("dragend1", event);
                this.dispatchEvent(new CustomEvent('markerDragend', { detail: { value: event.latLng } }));
            });

            this.markers.push(marker);

            return marker;
        }

        fnDeleteAllMarkers() {
            if (this.markers) {
                for (let i in this.markers) {
                    console.log("fnDeleteAllMarkers", this.markers[i]);
                    this.markers[i].setMap(null);
                }
                this.markers = [];
            }
        }

        fnClearData() {
            this.fnDeleteAllMarkers();
            this.fnDeleteAllPolygons();
        }

    }

);