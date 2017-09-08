(function () {
    var getAntiForgery = function () {
        return $('.js-analytics--anti-forgery').html();
    }

    var cleanUrl = function (url) {
        return url.replace(window.location.protocol + '//', '').replace(window.location.hostname, '').replace(':' + window.location.port, '');
    };

    var getUrl = function () {
        return cleanUrl(window.location.href);
    };

    var getData = function () {
        var data = '__RequestVerificationToken=' + encodeURIComponent(getAntiForgery()) +
            '&Url=' + encodeURIComponent(getUrl());
        return data;
    };

    var send = function () {
        var request = new XMLHttpRequest();
        request.open('POST', '/views/record', true);
        request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        request.send(getData());
    };

    var init = function () {
        send();
    };

    var ready = function (fn) {
        if (document.readyState != 'loading') {
            fn();
        } else if (document.addEventListener) {
            document.addEventListener('DOMContentLoaded', fn);
        } else {
            document.attachEvent('onreadystatechange', function () {
                if (document.readyState != 'loading')
                    fn();
            });
        }
    }

    ready(init);
}());