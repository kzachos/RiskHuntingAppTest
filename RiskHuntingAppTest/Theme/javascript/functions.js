var iWebkit;if(!iWebkit){iWebkit=window.onload=function(){function fullscreen(){var a=document.getElementsByTagName("a");for(var i=0;i<a.length;i++){if(a[i].className.match("noeffect")){}else{a[i].onclick=function(){window.location=this.getAttribute("href");return false}}}}function hideURLbar(){window.scrollTo(0,0.9)}iWebkit.init=function(){fullscreen();hideURLbar()};iWebkit.init()}}
	
function doLoad(url) {
document.getElementById("loading").innerHTML = "<div id=\'progress\'>Loading. Please wait. <img src=\'loader.gif\' /></div>";
window.location = url;
};

function ShowProgress() {
document.getElementById("loading").innerHTML = "<div id=\'progress\'>Loading. Please wait. <img src=\'loader.gif\' /></div>";
}

function ShowProgressSearch() {
document.getElementById("loading").innerHTML = "<div id=\'progress2\'>Searching. Please wait... <br><br> The Risk Hunting App is searching over 8000 risks and their resolutions documented in the plant. Generate new resolutions for your risk by: <br><br> 1. Reusing one or more previous resolutions, editing it as needed, or <br> 2. Using the creative prompts to generate new resolutions. <br> <img src=\'loader.gif\' /></div>";
}

function ShowProgressOLD() {
    setTimeout(function () {
        var modal = $('<div />');
        modal.addClass("modal");
        $('body').append(modal);
        var loading = $(".loading");
        loading.show();
        var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
        var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
        loading.css({ top: top, left: left });
    }, 200);
}


