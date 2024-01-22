using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;

// Copiedd script from https://www.youtube.com/watch?v=jhQkd6VRSJI

namespace Studio.Utilities
{
    public class TMPStyleSheetCreator : MonoBehaviour
    {
        public TMP_Text TextBox;
        public string OpeningTags;
        public string ClosingTags;

        public TextWeight TextWeight;

        [ContextMenu("Read Values From TMP")]
        public void ReadValuesFromTMP()
        {
            StringBuilder openSB = new StringBuilder();
            StringBuilder closeSB = new StringBuilder();

            if ((TextBox.fontStyle & FontStyles.Bold) != 0)
            {
                openSB.Append("<b>");
                closeSB.Append("</b>");
            }

            if ((TextBox.fontStyle & FontStyles.Italic) != 0)
            {
                openSB.Append("<i>");
                closeSB.Append("</i>");
            }

            if ((TextBox.fontStyle & FontStyles.UpperCase) != 0)
            {
                openSB.Append("<uppercase>");
                closeSB.Append("</uppercase>");
            }

            if ((TextBox.fontStyle & FontStyles.LowerCase) != 0)
            {
                openSB.Append("<lowercase>");
                closeSB.Append("</lowercase>");
            }

            if ((TextBox.fontStyle & FontStyles.SmallCaps) != 0)
            {
                openSB.Append("<smallcaps>");
                closeSB.Append("</smallcaps>");
            }

            if ((TextBox.fontStyle & FontStyles.Underline) != 0)
            {
                openSB.Append("<u>");
                closeSB.Append("</u>");
            }

            if ((TextBox.fontStyle & FontStyles.Strikethrough) != 0)
            {
                openSB.Append("<strikethrough>");
                closeSB.Append("</strikethrough>");
            }

            string fontAsset = TextBox.font.ToString();
            fontAsset = fontAsset.Replace("(TMPro.TMP_FontAsset)", string.Empty);

            openSB.Append($"<font=\"{fontAsset}\">");
            closeSB.Append("</font>");

            float textSize = TextBox.fontSize;
            openSB.Append($"<size={textSize}pt>");
            closeSB.Append("</size>");

            Color textColor = TextBox.color;
            string textColorRGB = ColorUtility.ToHtmlStringRGB(textColor);
            openSB.Append($"<color=#{textColorRGB}>");
            closeSB.Append("</color>");

            float characterSpacing = TextBox.characterSpacing;
            characterSpacing /= 100;
            string cSpacing = characterSpacing.ToString("N3", CultureInfo.InvariantCulture);

            if (characterSpacing != 0)
            {
                openSB.Append($"<cspace={cSpacing}em>");
                closeSB.Append("</cspace>");
            }

            float lineSpacing = TextBox.lineSpacing;
            lineSpacing /= 100;
            string lSpacing = lineSpacing.ToString("N3", CultureInfo.InvariantCulture);

            if (lineSpacing != 0)
            {
                openSB.Append($"<line-height={lSpacing}em>");
                closeSB.Append("</line-height>");
            }

            if (TextWeight == TextWeight.Black)
            {
                openSB.Append($"<font-weight={"900"}>");
                closeSB.Append("</font-weight>");
            }
            else if (TextWeight == TextWeight.Thin)
            {
                openSB.Append($"<font-weight={"100"}>");
                closeSB.Append("</font-weight>");
            }

            OpeningTags = openSB.ToString();
            ClosingTags = closeSB.ToString();
        }
    }

    public enum TextWeight
    {
        Regular,
        Thin,
        Black,
    }
}