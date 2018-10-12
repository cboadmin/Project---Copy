/*
| Returns base url
*/
function baseUrl() {
    var port = window.location.port != '' ? ':' + window.location.port : '';
    var baseUrl = window.location.protocol + '//' + window.location.hostname + port + '/';
    return baseUrl;
}

/*
| Gets the general key used to post to api
*/
function generalKey() {
    return 'GKEY1234567890';
}


/*
| Check if its email
*/
function isEmail(text) {

    var words = text.toLowerCase();
    var urlRegex = /(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))/;
    var match;

    if(match = urlRegex.exec(words)) {
        return true;
    }
    return false;
}

/*
| Gets value from query string
*/
function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? '' : sParameterName[1];
        }
    }

    return '';
};