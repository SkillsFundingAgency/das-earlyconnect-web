﻿function CookieBanner(module) {
    this.module = module;
    this.settings = {
        seenCookieName: 'ECSeenCookieMessage',
        env: window.GOVUK.getEnv(),
        cookiePolicy: {
            AnalyticsConsent: false
        }
    }
    if (!window.GOVUK.cookie(this.settings.seenCookieName + this.settings.env)) {
        this.start()
    }
}

CookieBanner.prototype.start = function () {
    this.module.cookieBanner = this.module.querySelector('.das-cookie-banner')
    this.module.cookieBannerInnerWrap = this.module.querySelector('.das-cookie-banner__wrapper')
    this.module.cookieBannerConfirmationMessage = this.module.querySelector('.das-cookie-banner__confirmation')
    this.module.cookieBannerRejectionMessage = this.module.querySelector('.das-cookie-banner__rejection')
    this.module.cookieBannerRejectionMessage = this.module.querySelector('.das-cookie-banner__rejection')
    this.setupCookieMessage()
}

CookieBanner.prototype.setupCookieMessage = function () {
    this.module.hideLinks = this.module.querySelectorAll('button[data-hide-cookie-banner]')
    this.module.acceptCookiesButton = this.module.querySelector('button[data-accept-cookies]')
    this.module.rejectCookiesButton = this.module.querySelector('button[data-reject-cookies]')

    this.module.hideLinks.forEach(button => {
        button.addEventListener('click', this.hideCookieBanner.bind(this))
    });

    if (this.module.acceptCookiesButton) {
        this.module.acceptCookiesButton.addEventListener('click', this.acceptAllCookies.bind(this))
    }

    if (this.module.rejectCookiesButton) {
        this.module.rejectCookiesButton.addEventListener('click', this.rejectAnalyticsCookies.bind(this))
    }

    this.showCookieBanner()
}

CookieBanner.prototype.showCookieBanner = function () {
    var cookiePolicy = this.settings.cookiePolicy,
        that = this;
    this.module.cookieBanner.style.display = 'block';
    this.module.cookieBannerConfirmationMessage.style.display = 'none'
    this.module.cookieBannerRejectionMessage.style.display = 'none'

    // Create the default cookies based on settings
    Object.keys(cookiePolicy).forEach(function (cookieName) {
        window.GOVUK.cookie(cookieName + that.settings.env, cookiePolicy[cookieName].toString(), { days: 365 })
    });

}

CookieBanner.prototype.hideCookieBanner = function () {
    this.module.cookieBanner.style.display = 'none';
    window.GOVUK.cookie(this.settings.seenCookieName + this.settings.env, true, { days: 365 })
}

CookieBanner.prototype.acceptAllCookies = function () {
    var that = this;
    this.module.cookieBannerInnerWrap.style.display = 'none';
    this.module.cookieBannerRejectionMessage.style.display = 'none';
    this.module.cookieBannerConfirmationMessage.style.display = 'block';

    window.GOVUK.cookie(this.settings.seenCookieName + this.settings.env, true, { days: 365 })

    Object.keys(this.settings.cookiePolicy).forEach(function (cookieName) {
        window.GOVUK.cookie(cookieName + that.settings.env, true, { days: 365 })
    });
}

CookieBanner.prototype.rejectAnalyticsCookies = function () {
    var that = this;
    this.module.cookieBannerInnerWrap.style.display = 'none';
    this.module.cookieBannerConfirmationMessage.style.display = 'none';
    this.module.cookieBannerRejectionMessage.style.display = 'block';

    window.GOVUK.cookie(this.settings.seenCookieName + this.settings.env, true, { days: 365 })

    Object.keys(this.settings.cookiePolicy).forEach(function (cookieName) {
        window.GOVUK.cookie(cookieName + that.settings.env, false, { days: 365 })
    });
}

window.GOVUK = window.GOVUK || {}

window.GOVUK.cookie = function (name, value, options) {
    if (typeof value !== 'undefined') {
        if (value === null) {
            return window.GOVUK.setCookie(name, '', { days: -1 })
        } else {
            // Default expiry date of 30 days
            if (typeof options === 'undefined') {
                options = { days: 30 }
            }
            return window.GOVUK.setCookie(name, value, options)
        }
    } else {
        return window.GOVUK.getCookie(name)
    }
}

window.GOVUK.setCookie = function (name, value, options) {

    if (typeof options === 'undefined') {
        options = {}
    }

    var cookieString = name + '=' + value + '; path=/;SameSite=None'

    if (options.days) {
        var date = new Date()
        date.setTime(date.getTime() + (options.days * 24 * 60 * 60 * 1000))
        cookieString = cookieString + '; expires=' + date.toGMTString()
    }

    if (!options.domain) {
        options.domain = window.GOVUK.getDomain()
    }

    if (document.location.protocol === 'https:') {
        cookieString = cookieString + '; Secure'
    }

    document.cookie = cookieString + ';domain=' + options.domain
}

window.GOVUK.getCookie = function (name) {
    var nameEQ = name + '='
    var cookies = document.cookie.split(';')
    for (var i = 0, len = cookies.length; i < len; i++) {
        var cookie = cookies[i]
        while (cookie.charAt(0) === ' ') {
            cookie = cookie.substring(1, cookie.length)
        }
        if (cookie.indexOf(nameEQ) === 0) {
            return decodeURIComponent(cookie.substring(nameEQ.length))
        }
    }
    return null
}

window.GOVUK.getDomain = function () {
    return window.location.hostname.indexOf('.') !== -1
        ? '.' + window.location.hostname.slice(window.location.hostname.indexOf('.') + 1)
        : window.location.hostname;
}

window.GOVUK.getEnv = function () {
    var domain = window.location.hostname;
    if (domain.indexOf("at-") >= 0) {
        return "AT"
    }
    if (domain.indexOf("test-") >= 0) {
        return "TEST"
    }
    if (domain.indexOf("test2-") >= 0) {
        return "TEST2"
    }
    if (domain.indexOf("pp-") >= 0) {
        return "PP"
    }
    return "";
}

// Legacy cookie clean up

var currentDomain = window.location.hostname;
var cookieDomain = window.GOVUK.getDomain();

if (currentDomain !== cookieDomain) {
    // Delete the 3 legacy cookies without the domain attribute defined
    document.cookie = "ECSeenCookieMessage=false; path=/;SameSite=None; expires=Thu, 01 Jan 1970 00:00:01 GMT";
    document.cookie = "AnalyticsConsent=false; path=/;SameSite=None; expires=Thu, 01 Jan 1970 00:00:01 GMT";
}

var $cookieBanner = document.querySelector('[data-module="cookie-banner"]');
if ($cookieBanner != null) {
    new CookieBanner($cookieBanner);
}

const CookieHandler = {
    init: function (buttonId, radioGroupName) {
        this.settings = {
            seenCookieName: 'ECSeenCookieMessage',
            env: window.GOVUK.getEnv(),
            cookiePolicy: {
                AnalyticsConsent: false
            }
        }

        const saveCookiesButton = document.getElementById(buttonId);
 
        if (saveCookiesButton) {
            saveCookiesButton.addEventListener('click', this.handleButtonClick.bind(this, radioGroupName));
        }

        //Pre-select the value matching the cookie
        var cookieValue = window.GOVUK.getCookie('AnalyticsConsent');
        var analyticsConsentValue = cookieValue && cookieValue == 'true' ? 'on':'off';

        const radioButtons = document.getElementsByName('cookies-AnalyticsConsent');
        for (const radioButton of radioButtons) {
            if (radioButton.value === analyticsConsentValue) {
                radioButton.checked = true;
                break; 
            }
        }    
    },

    handleButtonClick: function (radioGroupName) {
        const selectedRadioButton = document.querySelector(`input[name="${radioGroupName}"]:checked`);

        if (selectedRadioButton) {
            const selectedValue = (selectedRadioButton.value == 'on');
            window.GOVUK.cookie(this.settings.seenCookieName + this.settings.env, true, { days: 365 })
            Object.keys(this.settings.cookiePolicy).forEach(function (cookieName) {
                window.GOVUK.cookie(cookieName , selectedValue, { days: 365 });
            });
        }

        var cookieConfirmation = document.querySelector('.das-cookie-settings__confirmation');
        var cookieBanner = document.querySelector('.das-cookie-banner');

        cookieBanner.style.display = 'none';
        cookieConfirmation.style.display = 'block'
    }
};

// Usage example:
CookieHandler.init('btn-save-cookie-settings', 'cookies-AnalyticsConsent');

