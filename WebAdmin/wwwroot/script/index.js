// 提示
var Toast = (function () {
    function Toast(parameters) {
    }
    Toast.Success = function (msg) {
        return this.ActionInfo(msg, "success");
    };
    Toast.Error = function (msg) {
        return this.ActionInfo(msg, "error");
    };
    Toast.Info = function (msg) {
        return this.ActionInfo(msg, "info");
    };
    Toast.ActionInfo = function (msg, type) {
        if (type === void 0) { type = "success"; }
        $(".toast").hide();
        var toastTmp;
        var divClass;
        var msgClass;
        switch (type) {
            case "info":
                divClass = "info";
                msgClass = "info";
                break;
            case "error":
                divClass = "danger";
                msgClass = "danger";
                break;
            case "success":
                divClass = "success";
                msgClass = "success";
                break;
            default:
                break;
        }
        position = "style='position:absolute;right: 2px;bottom: 20px;width: 240px;'";
        toastTmp = "\n        <div class=\"alert alert-"
            + divClass
            + " toast\""
            + position
            + " role=\"alert\">\n        <button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>\n            <p class=\"text-"
            + msgClass + "\">\n            <strong>"
            + type + "</strong>\n            </br>\n            "
            + msg + "\n            </p>\n\n        </div>\n        ";
        $("body").append(toastTmp);
        setTimeout(function () {
            $(".toast").slideUp(1000);
        }, 3000);
    };
    return Toast;
}());

function reloadPage(time = 3000) {

    window.setTimeout(function () {
    }, time)
}