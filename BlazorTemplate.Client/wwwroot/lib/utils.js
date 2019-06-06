window.reloadPopper = function ()
{
    $('[data-toggle="tooltip"]').tooltip();

    return true;
}

window.reloadSideBar = function ()
{
    $('#perfect-scrollbar-nav').sidebar();
    //$('.sidebar').perfectScrollbar = new PerfectScrollbar('#perfect-scrollbar-nav');

    return true;
}

getCookies = function ()
{
    return Promise.resolve(document.cookie);
}

showRegisterModal = function ()
{
    $('#registerModal').modal('show');
    
    return true;
}

copyToClipBoard = function (target)
{
    document.getElementById(target).select();
    document.execCommand("copy");

    return true;
}