using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {

    public class ItemSlotTheme {
        private Color panelColor;
        private Color outlineColor;

        public ItemSlotTheme(Color panelColor, Color outlineColor)
        {
            this.panelColor = panelColor;
            this.outlineColor = outlineColor;
        }
        public Color PanelColor { get => panelColor; }
        public Color OutlineColor { get => outlineColor; }
    }
    public class ColorTheme
    {
        private Color primary;
        private Color secondary;
        private Color tertiary;
        private Color foreground;
        private Color highlight;
        private Color highlightText;
        private Color text;
        private ItemSlotTheme itemSlotTheme;
        public Color Primary { get => primary; }
        public Color Secondary { get => secondary;}
        public Color Tertiary { get => tertiary; }
        public Color Foreground { get => foreground; }
        public Color Highlight { get => highlight; }
        public Color HighlightText { get => highlightText; }
        public Color Text { get => text;}
        public ItemSlotTheme ItemSlotTheme {get => itemSlotTheme;}
        
        public ColorTheme(string primary, string secondary, string tertiary, string foreground, string highlight, string highlightText, string textColor)
        {
            this.primary = GlobalUtils.fromHex(primary);
            this.secondary = GlobalUtils.fromHex(secondary);
            this.tertiary = GlobalUtils.fromHex(tertiary);
            this.foreground = GlobalUtils.fromHex(foreground);
            this.highlight = GlobalUtils.fromHex(highlight);
            this.highlightText = GlobalUtils.fromHex(highlightText);
            this.text = GlobalUtils.fromHex(textColor);
            this.itemSlotTheme = new ItemSlotTheme(
                GlobalUtils.fromHex(secondary),
                GlobalUtils.fromHex(highlight)
            );
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
            this.itemSlotTheme = new ItemSlotTheme(
                GlobalUtils.fromHex(secondary),
                GlobalUtils.fromHex(highlight)
            );
        }

        public ColorTheme(string primary, string secondary, string tertiary, string foreground, string highlight, ItemSlotTheme itemSlotTheme)
        {
            this.primary = GlobalUtils.fromHex(primary);
            this.secondary = GlobalUtils.fromHex(secondary);
            this.tertiary = GlobalUtils.fromHex(tertiary);
            this.foreground = GlobalUtils.fromHex(foreground);
            this.highlight = GlobalUtils.fromHex(highlight);
            this.highlightText = GlobalUtils.fromHex(highlight);
            this.text = GlobalUtils.fromHex(secondary);
            this.itemSlotTheme = itemSlotTheme;
        }

        public ColorTheme(string primary, string secondary, string tertiary, string foreground, string highlight, string highlightText, string text, ItemSlotTheme itemSlotTheme)
        {
            this.primary = GlobalUtils.fromHex(primary);
            this.secondary = GlobalUtils.fromHex(secondary);
            this.tertiary = GlobalUtils.fromHex(tertiary);
            this.foreground = GlobalUtils.fromHex(foreground);
            this.highlight = GlobalUtils.fromHex(highlight);
            this.highlightText = GlobalUtils.fromHex(highlightText);
            this.text = GlobalUtils.fromHex(text);
            this.itemSlotTheme = itemSlotTheme;
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
                textColor: "F2F3F4"
            );
        }
    }
}

