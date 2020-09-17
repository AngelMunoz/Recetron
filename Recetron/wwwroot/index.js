Recetron = {
    Interop: {
        /**
         * Checks if the browser has the Share API
         * @returns {Promise<boolean>}
         */
        hasShareAPI() {
            if (window.navigator.share && window.navigator.canShare) {
                return window.navigator.canShare({
                    title: '',
                    text: '',
                    url: ''
                });
            }
            return Promise.resolve(false);
        },
        /**
         * function used to share text where the devices have the share API available
         * @param {string} title title of the resource being shared
         * @param {string} text either full content or a small description
         * @param {string?} url URL that can be used to visit the resource being shared
         * @return {Promise<void>}
         */
        shareMobile(title, text, url = null) {
            if (window.navigator.share) {
                return window.navigator.share({ title, text, url });
            }
            // not supported
            return Promise.reject(new Error('Sharing is not Supported'));
        }

    }
}



