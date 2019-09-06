(function (window, undefined) {
    var listM = [];
    ThreeMapInit = function (floorInfo, labelArray, $ref) {
            //数据处理  X,Z,Y //由于图像原因  Y轴为负值
            for (var i = 0; i < labelArray.length; i++) {
                var item = JSON.parse(labelArray[i].POINTS);
                labelArray[i].POINTS = [item[0], 6.5, -item[1]];
            }            
            for (var i = 0; i < floorInfo.MAPSHOPLIST.length; i++) {
                var pointlist = JSON.parse(floorInfo.MAPSHOPLIST[i].POINTS);
                for(var j = 0; j < pointlist.length; j++){
                    pointlist[j] = [pointlist[j][0], 0, -pointlist[j][1]];
                }
                floorInfo.MAPSHOPLIST[i].POINTS=pointlist;
            }

            var renderer, scene, camera;
            var INTERSECTED;
            var raycaster;
            var mouse;
            if (listM.length > 0) {
                for (var i = 0; i < listM.length; i++) {
                    var mesh = listM[i];
                    mesh.geometry.dispose();
                    mesh.material.dispose();
                }
            }

            var controls;
            // 边框线的高度
            var lineHeight = 1.75;
            // 块的高度
            var cubeHeight = 1.5;
            //获取容器的宽高
            var $tree = $ref.tree;  //左面目标元素
            var $map = $ref.maps;    //右面目标元素
            var width = $map.clientWidth;     //获取画布「canvas3d」的宽
            var height = window.innerHeight - 22;   //获取画布「canvas3d」的高
            renderer = new THREE.WebGLRenderer({
                antialias: true
            });
            clearRenderer();

            function init() {
                renderer = new THREE.WebGLRenderer({
                    antialias: true
                });
              
                renderer.setClearColor(0xF1F2F7);
                renderer.setSize(width, height);
                scene = new THREE.Scene();
                scene.background = new THREE.Color(0xF1F2F7);
                camera = new THREE.PerspectiveCamera(50, width / height, 0.1, 1000);
                camera.lookAt(new THREE.Vector3(20, 0, 20));
                camera.position.set(0, 200, 0);    //生成可视角度
                // 光线的照射
                var ambiColor = "#FFFFFF";
                var light = new THREE.AmbientLight(ambiColor,0.6); // 环境光
                scene.add(light);
                var spotLight = new THREE.SpotLight(ambiColor);// 射灯光
                spotLight.position.set(0, 150,0);
                scene.add(spotLight);
                //var spotLight2 = new THREE.SpotLight(ambiColor);
                //spotLight2.position.set(100, 100, 150);
                //scene.add(spotLight2);

                controls = new THREE.OrbitControls(camera, renderer.domElement);//用户交互

                window.ThreeMapcontrols = controls;     //外放 控制器，，  可修改操控参数

                //设置相机的角度范围
                controls.maxPolarAngle = Math.PI * 0.5;
                //设置相机距离原点的最远距离
                controls.minDistance = 0;
                //设置相机距离原点的最远距离
                controls.maxDistance = 500;

                raycaster = new THREE.Raycaster();
                mouse = new THREE.Vector2();
                $map.appendChild(renderer.domElement);
                $map.addEventListener('click', onDocumentMouseClick, false);   //鼠标单击方法 
                $map.addEventListener('dblclick', onDocumentMouseDBLClick, false);   //鼠标双击方法


                //楼层以及子项信息
                initGroup();

                var floor = new loadFloor(floorInfo);
                floor.load();

                for (var i = 0; i < labelArray.length; i++) {
                    addLabelSprite(labelArray[i].SHOPNAME, 13, labelArray[i].POINTS);
                }

                render();
            }

            //楼层子项的分类类型
            var ObjType = {
                FLOOR: "floor",        //地板
                CELL: "shop",          //常用的小隔间
            }

            var floorGroup;     //楼层元素组
            var labelGroup;     //楼层标注组

            //初始化一些必要的组
            function initGroup() {
                floorGroup = new THREE.Object3D();
                scene.add(floorGroup);

                labelGroup = new THREE.Object3D();
                scene.add(labelGroup);
            }

            //初始化楼层数据
            var loadFloor = function (floor) {
                if (!floorGroup) {
                    floorGroup = new THREE.Object3D();
                    scene.add(floorGroup);
                }
                this.container = floorGroup;            //存放楼层元素的容器
                this.data = floor;
            }

            //加载楼层
            loadFloor.prototype.load = function () {
                var floor = this.data;
                var Branchid = floor.BRANCHID, Regionid = floor.REGIONID; floorid = floor.FLOORID;

                var buidlingItem = floor.MAPSHOPLIST;
                for (var i = 0; i < buidlingItem.length; i++) {
                    var item = buidlingItem[i];
                    var type = item.TYPE;
                    var points = item.POINTS;
                    var color = item.COLOR;
                    switch (type) {
                        case ObjType.CELL:
                            this.addCell(points, 6, color, item.SHOPINFO);
                            break;
                        case ObjType.FLOOR:
                            this.addFloor(points, 0.5, item.SHOPINFO);
                            break;
                    }
                }
            }

            /*
            *   创建地板
            * */
            loadFloor.prototype.addFloor = function (points, height, info) {
                var geometry = this.getGeometry(points, height, info);
                geometry.computeFaceNormals();          //计算法向量
                var material = new THREE.MeshLambertMaterial({ color: "#C1C1C1", side: THREE.DoubleSide });         //MeshLambertMaterial
                var mesh = new THREE.Mesh(geometry, material);
                mesh.castShadow = true;
                listM.push(mesh);
                this.container.add(mesh);				//添加填充
            }

            /*
            *   创建不规则的小室
            * */
            loadFloor.prototype.addCell = function (points, height, color, info) {
                var geometry = this.getGeometry(points, height, info);
                geometry.computeFaceNormals();          //计算法向量
                var material;
                if (info.RENT_STATUS == "1") {
                    material = new THREE.MeshLambertMaterial({ color: "#C3C1BD", side: THREE.DoubleSide });         //模块颜色    单元空置
                } else {
                    material = new THREE.MeshLambertMaterial({ color: color, side: THREE.DoubleSide });         //模块颜色
                }
                var mesh = new THREE.Mesh(geometry, material);
                listM.push(mesh);
                this.container.add(mesh);				//添加填充

                var lineMaterial = new THREE.LineBasicMaterial({ color: "#F7A540" });     //线颜色
                var lineGeometry = this.getGeometry(points, height, info);
                var line = new THREE.Line(lineGeometry, lineMaterial);
                listM.push(mesh);
                this.container.add(line);
            }

            //生成顶部的线
            loadFloor.prototype.getBorderGeometry = function (points, height, color) {
                var geometry = new THREE.Geometry();
                var topPoints = [];
                for (var i = 0; i < points.length; i++) {
                    var vertice = points[i];
                    topPoints.push([vertice[0], vertice[1] + height, vertice[2]]);
                }
                for (var i = 0; i < topPoints.length; i++) {
                    var topPoint = topPoints[i];
                    geometry.vertices.push(new THREE.Vector3(topPoint[0], topPoint[1], topPoint[2]));
                    if (i == topPoints.length - 1) {
                        geometry.vertices.push(new THREE.Vector3(topPoints[0][0], topPoints[0][1], topPoints[0][2]));
                    }
                }
                return geometry;
            }

            //根据传入的坐标点获取几何
            loadFloor.prototype.getGeometry = function (points, height, info) {
                
                if (points.length < 3) {
                    console.log("至少需要三个点来创建盒子");
                    return;
                }
                var topPoints = [];
                for (var i = 0; i < points.length; i++) {
                    var vertice = points[i];
                    topPoints.push([vertice[0], height, vertice[2]]);
                }
                var totalPoints = points.concat(topPoints);
                var vertices = [];
                for (var i = 0; i < totalPoints.length; i++) {
                    vertices.push(new THREE.Vector3(totalPoints[i][0], totalPoints[i][1], totalPoints[i][2]))
                }
                var length = points.length;
                var faces = [];
                for (var j = 0; j < length; j++) {                      //侧面生成三角形
                    if (j != length - 1) {
                        faces.push(new THREE.Face3(j, j + 1, length + j + 1));
                        faces.push(new THREE.Face3(length + j + 1, length + j, j));
                    } else {
                        faces.push(new THREE.Face3(j, 0, length));
                        faces.push(new THREE.Face3(length, length + j, j));
                    }
                }
                var data = [];
                for (var i = 0; i < length; i++) {
                    data.push(points[i][0], points[i][2]);
                }
                var triangles = triangulate(data);
                if (triangles && triangles.length != 0) {
                    for (var i = 0; i < triangles.length; i++) {
                        var tlength = triangles.length;
                        if (i % 3 == 0 && i < tlength - 2) {
                            faces.push(new THREE.Face3(triangles[i], triangles[i + 1], triangles[i + 2]));                            //底部的三角面
                            faces.push(new THREE.Face3(triangles[i] + length, triangles[i + 1] + length, triangles[i + 2] + length));        //顶部的三角面
                        }
                    }
                }
                var geometry = new THREE.Geometry();
                geometry.vertices = vertices;
                geometry.faces = faces;
                geometry.name = info;
                return geometry;
            }

            //清除楼层
            loadFloor.prototype.clear = function () {
                scene.remove(this.container);
                this.container = null;
                floorGroup = null;
            }


            var selectedCell;       //选中的房间
            var instantMaterial;    //储存选中mesh的材质
            //鼠标单击事件
            function onDocumentMouseClick(event) {
                event.preventDefault();
                var treew = $tree.clientWidth;   //左侧树宽度
                var vector = new THREE.Vector3();//三维坐标对象
                vector.set(
                        ((event.clientX - treew - 16) / width) * 2 - 1,      //16是margin 的距离
                        -(event.clientY / height) * 2 + 1,
                        0.5);
                vector.unproject(camera);
                var raycaster = new THREE.Raycaster(camera.position, vector.sub(camera.position).normalize());
                var intersects = raycaster.intersectObjects(scene.children[2].children);
                if (intersects.length) {      //有时先选到线,选不中
                    var point = intersects[0].point;
                    //console.log(point.x + "," + point.y + "," + point.z);
                    if (selectedCell) {
                        selectedCell.material = instantMaterial;
                    }
                    for (var i = 0; i < intersects.length;i++){
                        if (intersects[i].object.type == "Mesh") {   //判断选中得是线还是模块
                            selectedCell = intersects[i].object;//取第一个物体 
                            break;
                        }
                    }
                    instantMaterial = selectedCell.material;
                    if (selectedCell.geometry.name.TYPE=="shop") {   //点击地板不要动作
                        selectedCell.material = new THREE.MeshBasicMaterial({ color: "#BF5430", side: THREE.DoubleSide });  //选中改变颜色
                        //console.log(selectedCell.geometry.name);   //选中模块名称   暂时只能自定义name  
                    }
                } else {
                    if (selectedCell) {
                        selectedCell.material = instantMaterial;
                        selectedCell = null;
                    }
                }
                mouse.x = (event.clientX / window.innerWidth) * 2 - 1;
                mouse.y = -(event.clientY / window.innerHeight) * 2 + 1;
            }

            //鼠标双击事件
            function onDocumentMouseDBLClick(event) {
                event.preventDefault();
                var treew = $tree.clientWidth; //左侧树宽度
                var vector = new THREE.Vector3();//三维坐标对象
                vector.set(
                        ((event.clientX - treew-16 )/ width) * 2 - 1,    //16是margin 的距离
                        -(event.clientY / height) * 2 + 1,
                        0.5);
                vector.unproject(camera);
                var raycaster = new THREE.Raycaster(camera.position, vector.sub(camera.position).normalize());
                var intersects = raycaster.intersectObjects(scene.children[2].children);
                if (intersects.length) {      //有时先选到线,选不中
                    var point = intersects[0].point;
                    //console.log(point.x + "," + point.y + "," + point.z);
                    if (selectedCell) {
                        selectedCell.material = instantMaterial;
                    }
                    for (var i = 0; i < intersects.length; i++) {
                        if (intersects[i].object.type == "Mesh") {   //判断选中得是线还是模块
                            selectedCell = intersects[i].object;//取第一个物体 
                            break;
                        }
                    }
                    instantMaterial = selectedCell.material;
                    if (selectedCell.geometry.name.TYPE == "shop") {   //点击地板不要动作
                        selectedCell.material = new THREE.MeshBasicMaterial({ color: "#BF5430", side: THREE.DoubleSide });  //选中改变颜色
                        //console.log(selectedCell.geometry.name);
                        // 点击展示商铺信息
                        ThreeMapClick(selectedCell.geometry.name.ID);
                    }
                } else {
                    if (selectedCell) {
                        selectedCell.material = instantMaterial;
                        selectedCell = null;
                    }
                }
                mouse.x = (event.clientX / window.innerWidth) * 2 - 1;
                mouse.y = -(event.clientY / window.innerHeight) * 2 + 1;
            }


            //使用sprite制作标注
            function addLabelSprite(message, fontsize, position) {
                var canvas = generateCanvas(message);
                makeCanvasSprite(canvas, position);
            }

            //根据文字生产canvas
            function generateCanvas(text) {
                var canvas = document.createElement('canvas');
                var context = canvas.getContext('2d');
                context.font='50px Microsoft YaHei';
                canvas.width = context.measureText(text).width;      //根据文字内容获取宽度
                canvas.height = 58; // fontsize * 1.5
                context.beginPath();
                context.font = '50px Microsoft YaHei';
                context.fillStyle = "#222";
                context.fillText(text, 0, 50);
                context.fill();
                context.stroke();
                return canvas;
            }

            //根据canvas图形制作sprite
            function makeCanvasSprite(canvas, position) {
                var texture = new THREE.Texture(canvas);
                texture.needsUpdate = true;
                //var map = new THREE.TextureLoader().load("../../Content/three/a.png");     //图片标注
                //var spriteMaterial = new THREE.SpriteMaterial({ map: map, depthWrite: false });
                var spriteMaterial = new THREE.SpriteMaterial({ map: texture, depthWrite: false });
                var sprite = new THREE.Sprite(spriteMaterial);
                sprite.renderOrder = 0;
                sprite.center = new THREE.Vector2(0.5, 0);
                sprite.position.x = position[0];
                sprite.position.y = position[1];
                sprite.position.z = position[2];
                //sprite.visible = false;
                labelGroup.add(sprite);
            }

            function updateLabel() {
                var labelSprites = labelGroup ? labelGroup.children : [];
                for (var i = 0; i < labelSprites.length; i++) {
                    var sprite = labelSprites[i];
                    var v = new THREE.Vector3();
                    var scale_factor = 3;
                    sprite.scale.x = sprite.scale.y = v.subVectors(sprite.position, camera.position).length() / scale_factor;
                }
            }

            //更新文字sprite标注
            function updateLabelSprite() {
                var sprites = labelGroup ? labelGroup.children : [];
                if (sprites.length == 0) return;
                for (var i = 0; i < sprites.length; i++) {
                    var sprite = sprites[i];
                    sprite.visible = true;
                }

                for (var i = 0; i < sprites.length; i++) {
                    var compareSprite = sprites[i];
                    var canvas = compareSprite.material.map.image;
                    if (canvas) {
                        var position = compareSprite.position;
                        var scale = getPoiScale(position, { w: canvas.width, h: canvas.height });
                        compareSprite.scale.set(scale[0] / 4, scale[1] / 4, 1.0);
                        if (compareSprite.visible) {		//true
                            for (var j = i + 1; j < sprites.length; j++) {
                                var sprite = sprites[j];
                                if (isPOILabelRect(compareSprite, sprite)) {
                                    sprite.visible = false;
                                }
                            }
                        }
                    }
                }
            }

            function getPoiScale(position, poiRect) {
                if (!position) return;
                var distance = camera.position.distanceTo(position);        //相机和标注点的距离
                var top = Math.tan(camera.fov / 2 * Math.PI / 180) * distance;    //camera.fov 相机的拍摄距离
                var meterPerPixel = 2 * top / window.innerHeight;
                var scaleX = poiRect.w * meterPerPixel;
                var scaleY = poiRect.h * meterPerPixel;
                return [scaleX, scaleY, 1.0];
            }

            //检测两个标注sprite是否碰撞
            function isPOILabelRect(compareSprite, sprite) {
                var comparePosition = toScreenPos([compareSprite.position.x, compareSprite.position.y, compareSprite.position.z], camera);
                var spritePosition = toScreenPos([sprite.position.x, sprite.position.y, sprite.position.z], camera);

                var image1 = compareSprite.material.map.image;
                var image2 = sprite.material.map.image;
                var w1 = image1.width / 2;
                var h1 = image1.height / 2;
                var x1 = comparePosition.x - w1 / 2;
                var y1 = comparePosition.y - h1 / 2;

                var w2 = image2 ? image2.width / 2 : 0;
                var h2 = image2 ? image2.height / 2 : 0;

                var x2 = spritePosition.x - w2 / 2;         //点2左下角的xy点
                var y2 = spritePosition.y - h2 / 2;
                if (x1 >= x2 && x1 >= x2 + w2) {
                    return false;
                } else if (x1 <= x2 && x1 + w1 <= x2) {
                    return false;
                } else if (y1 >= y2 && y1 >= y2 + h2) {
                    return false;
                } else if (y1 <= y2 && y1 + h1 <= y2) {
                    return false;
                } else {
                    return true;
                }
            }

            //three世界坐标转为屏幕坐标
            function toScreenPos(position, camera) {
                var worldVector = new THREE.Vector3(
                        position[0],
                        position[1],
                        position[2]
                );
                var standardVector = worldVector.project(camera);//世界坐标转标准设备坐标
                var a = window.innerWidth / 2;
                var b = window.innerHeight / 2;
                var x = Math.round(standardVector.x * a + a);//标准设备坐标转屏幕坐标
                var y = Math.round(-standardVector.y * b + b);//标准设备坐标转屏幕坐标
                return {
                    x: x,
                    y: y
                };
            }
            function clearRenderer() {
                renderer.dispose();
                renderer.forceContextLoss();
                renderer.context = null;
                renderer.domElement = null;
                renderer = null;
            }
        ////外放方法
        //    function ThreeMapClick(id) {
        //        alert(id);
        //    };

            function render() {
                controls.update();
                requestAnimationFrame(render);
                renderer.render(scene, camera);
                updateLabelSprite();
            }
            init();
    }

})(window);