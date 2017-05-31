(function (jQuery) {

    jQuery.extend({
        fsPopup: {
            repository: [],

            top: 0,

            popup: function (url, callback, option) {
                var topWindow = this.getTopWindow();

                if (topWindow != window) {
                    return topWindow.jQuery.fsPopup.popup(url, callback, option);
                }

                var iFrame = jQuery(document.createElement('iframe'));
                iFrame.attr({ src: url });
                //$("body").append(iFrame);
                iFrame.dialog(option);
                iFrame.dialog('open');
                this.repository[this.top++] = {
                    windoo: iFrame,
                    callback: callback
                };

                //iFrame.context.style.background = "white";
                iFrame.context.style.width = "96%";
                iFrame.context.style.height = "96%";
                iFrame.context.style.borderStyle = "none";
                iFrame.context.style.padding = "0";
                if ((option['width'] != null) && (option['width'] != 'undefined')) {
                    iFrame.context.style.minWidth = option['width'] + 'px';
                    iFrame.context.style.width = option['width'] + 'px';
                }
                if ((option['height'] != null) && (option['height'] != 'undefined')) {
                    iFrame.context.style.minHeight = (option['height'] - 36) + 'px';
                    iFrame.context.style.height = (option['height'] - 36) + 'px';
                }
            },


            close: function (action, value) {


                var topWindow = this.getTopWindow();

                if (topWindow != window) {
                    topWindow.jQuery.fsPopup.close(action, value);
                    return;
                }


                if (this.top == 0) return;
                var repoItem = this.repository[--this.top];

                //repoItem.windoo.dialog('close');
                //repoItem.windoo.remove();
                repoItem.windoo.modal = false;
                repoItem.windoo.dialog('close', this.top);

                var func = repoItem.callback;
                func(action, value);
            },

            getTopWindow: function () {
                var currentWindow = window;

                while (currentWindow.parent != currentWindow) {
                    currentWindow = currentWindow.parent;
                }

                return currentWindow;
            }
        }
    });

})(jQuery);