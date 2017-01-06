//共享onload事件
function addLoadEvent(func) {
    var oldonload = window.onload;
    if (typeof window.onload != 'function') {
        window.onload = func;
    } else {
        window.onload = function() {
            oldonload();
            func();
        }
    }
}

//插入新元素到指定元素之后
function insertAfter(newElement, targetElement) {
    var parent = targetElement.parentNode;
    if (parent.lastChild == targetElement) {
        parent.appendChild(newElement);
    } else {
        parent.insertBefore(newElement, targetElement.nextSibling);
    }
}

//原生AJAX
function getHTTPObject() {
    // ie不支持XMLHttpRequest，需要做兼容
    // 判断是否支持XMLHttpRequest对象
    if (tyoeof XMLHttpRequest == 'undefined') {
        XMLHttpRequest = function() {
            try {
                return new ActiveXObject("Msxml2.XMLHTTP.6.0");
            } catch (e) {}
            try {
                return new ActiveXObject("Msxml2.XMLHTTP.3.0");
            } catch (e) {}
            try {
                return new ActiveXObject("Msxml2.XMLHTTP");
            } catch (e) {}
            return false;
        }
    }
    return new XMLHttpRequest();
}

// 调用请求
function getNewContent() {
    var request = getHTTPObject();
    if (request) {
        request.open("GET", "example.txt", true);
        request.onreadystatechange = function() {
            //readyState有5个可能的值
            //0 -- 未初始化
            //1 -- 正在加载
            //2 -- 加载完成
            //3 -- 正在交互
            //4 -- 完成
            if (request.readyState == 4) {
                var para = document.createElement("p");
                var txt = document.createTextNode(request.responseText);
                para.appendChild(txt);
                document.getElementById('new').appendChild(para);
            }            
        };
        request.send(null);
    }else{
        alert('Sorry, your browser does\'t support XMLHttpRequest');
    }
}

addLoadEvent(getNewContent);