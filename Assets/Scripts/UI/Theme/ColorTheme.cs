using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public class ColorTheme
    {
        private Color primary;
        private Color secondary;
        private Color tertiary;
        private Color foreground;
        private Color highlight;
        private Color highlightText;
        private Color text;

        public Color Primary { get => primary; }
        public Color Secondary { get => secondary;}
        public Color Tertiary { get => tertiary; }
        public Color Foreground { get => foreground; }
        public Color Highlight { get => highlight; }
        public Color HighlightText { get => highlightText; }
        public Color Text { get => text;}

        public ColorTheme(string primary, string secondary, string tertiary, string foreground, string highlight, string highlightText, string text)
        {
            this.primary = GlobalUtils.fromHex(primary);
            this.secondary = GlobalUtils.fromHex(secondary);
            this.tertiary = GlobalUtils.fromHex(tertiary);
            this.foreground = GlobalUtils.fromHex(foreground);
            this.highlight = GlobalUtils.fromHex(highlight);
            this.highlightText = GlobalUtils.fromHex(highlightText);
            this.text = GlobalUtils.fromHex(text);
        }

        public ColorTheme(string primary, string secondary, string tertiary, string foreground, string highlight)
        {
            this.primary = GlobalUtils.fromHex(primary);
            this.secondary = GlobalUtils.fromHex(secondary);
            this.tertiary = GlobalUtils.fromHex(tertiary);
            this.foreground = GlobalUtils.fromHex(foreground);
            this.highlight = GlobalUtils.fromHex(highlight);
            this.highlightText = GlobalUtils.fromHex(highlight);
            this.text = GlobalUtils.fromHex(secondary);
        }
    }

    

    public static class ColorThemeUtils {
        public static ColorTheme InventoryTheme {
            get => new ColorTheme(
                primary: "003B36",
                secondary: "003B36F8",
                tertiary: "E98A15",
                foreground: "012622",
                highlight: "59114D",
                highlightText: "E98A15",
                text: "F2F3F4"
            );
        }
    }
}

