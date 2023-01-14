using UnityEngine;

public static class RectTransformExtensions {
    public static void AnchorToCanvas(this RectTransform RectTransform, TextAnchor Anchor, float Width, float Height) {
        if (Width < 0 && Height < 0) {
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.one;
            RectTransform.pivot = new Vector2(0.5f, 0.5f);
        } else {
            switch (Anchor) {
                case TextAnchor.LowerLeft:
                    RectTransform.anchorMin = Vector2.zero;
                    RectTransform.anchorMax = Vector2.zero;
                    RectTransform.pivot = Vector2.zero;
                    break;
                case TextAnchor.LowerCenter:
                    RectTransform.anchorMin = new Vector2(0.5f, 0f);
                    RectTransform.anchorMax = new Vector2(0.5f, 0f);
                    RectTransform.pivot = new Vector2(0.5f, 0f);
                    break;
                case TextAnchor.LowerRight:
                    RectTransform.anchorMin = new Vector2(1f, 0f);
                    RectTransform.anchorMax = new Vector2(1f, 0f);
                    RectTransform.pivot = new Vector2(1f, 0f);
                    break;
                case TextAnchor.MiddleLeft:
                    RectTransform.anchorMin = new Vector2(0f, 0.5f);
                    RectTransform.anchorMax = new Vector2(0f, 0.5f);
                    RectTransform.pivot = new Vector2(0f, 0.5f);
                    break;
                case TextAnchor.MiddleCenter:
                    RectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    RectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    RectTransform.pivot = new Vector2(0.5f, 0.5f);
                    break;
                case TextAnchor.MiddleRight:
                    RectTransform.anchorMin = new Vector2(1f, 0.5f);
                    RectTransform.anchorMax = new Vector2(1f, 0.5f);
                    RectTransform.pivot = new Vector2(1f, 0.5f);
                    break;
                case TextAnchor.UpperLeft:
                    RectTransform.anchorMin = new Vector2(0f, 1f);
                    RectTransform.anchorMax = new Vector2(0f, 1f);
                    RectTransform.pivot = new Vector2(0f, 1f);
                    break;
                case TextAnchor.UpperCenter:
                    RectTransform.anchorMin = new Vector2(0.5f, 1f);
                    RectTransform.anchorMax = new Vector2(0.5f, 1f);
                    RectTransform.pivot = new Vector2(0.5f, 1f);
                    break;
                case TextAnchor.UpperRight:
                    RectTransform.anchorMin = new Vector2(1f, 1f);
                    RectTransform.anchorMax = new Vector2(1f, 1f);
                    RectTransform.pivot = new Vector2(1f, 1f);
                    break;
                default:
                    break;
            }

            if (Width < 0 || Height < 0) {
                RectTransform.anchorMin = new Vector2(Height < 0 ? RectTransform.anchorMin.x : 0f, Width < 0 ? RectTransform.anchorMin.y : 0f);
                RectTransform.anchorMax = new Vector2(Height < 0 ? RectTransform.anchorMax.x : 1f, Width < 0 ? RectTransform.anchorMax.y : 1f);
            }
        }
    }

    public static float CorrectedX(this RectTransform RectTransform) {
        float ScaledX = RectTransform.position.x;

        float AspectRatio = (float)Screen.width/Screen.height;
        float AspectRatioDesign = (800f/600f);
        float Scale = Screen.width/800f*AspectRatioDesign/AspectRatio;
        if (Scale == 0f) Scale = 1f;

        float CorrectedX = ScaledX/Scale;

        return CorrectedX;
    }

    public static float CorrectedY(this RectTransform RectTransform) {
        float ScaledY = RectTransform.position.y;
        float Scale = Screen.height/600f;
        if (Scale == 0f) Scale = 1f;

        float CorrectedY = ScaledY/Scale;

        return CorrectedY;
    }

    public static void Offset(this RectTransform RectTransform, float PositionX, float PositionY, TextAnchor Anchor, Vector2 Margin) {
        switch (Anchor) {
            case TextAnchor.LowerLeft:
                Margin = new Vector2(Margin.x, -Margin.y);
                break;
            case TextAnchor.LowerCenter:
                Margin = new Vector2(0f, -Margin.y);
                break;
            case TextAnchor.LowerRight:
                Margin = new Vector2(-Margin.x, -Margin.y);
                break;
            case TextAnchor.MiddleLeft:
                Margin = new Vector2(Margin.x, 0f);
                break;
            case TextAnchor.MiddleCenter:
                Margin = new Vector2(0f, 0f);
                break;
            case TextAnchor.MiddleRight:
                Margin = new Vector2(-Margin.x, 0f);
                break;
            case TextAnchor.UpperLeft:
                Margin = new Vector2(Margin.x, Margin.y);
                break;
            case TextAnchor.UpperCenter:
                Margin = new Vector2(0f, Margin.y);
                break;
            case TextAnchor.UpperRight:
                Margin = new Vector2(-Margin.x, Margin.y);
                break;
            default:
                break;
        }

        RectTransform.anchoredPosition = new Vector2(PositionX + Margin.x, PositionY - Margin.y);
    }

    public static void Offset(this RectTransform RectTransform, float PositionX, float PositionY) {
        RectTransform.anchoredPosition = new Vector2(PositionX, PositionY);
    }

    public static void ResizeTo(this RectTransform RectTransform, float Width, float Height, Vector2 Margin) {
        //Resize
        if (Height > 0) {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Height);
        } else {
            RectTransform.offsetMin = new Vector2(RectTransform.offsetMin.x, Margin.y);
        }

        if (Width > 0) {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width);
        } else {
            RectTransform.offsetMax = new Vector2(-Margin.x, RectTransform.offsetMax.y);
        }
    }

    public static void SetMargin(this RectTransform RectTransform, Vector2 Margin) {
        RectTransform.offsetMin = Margin;
        RectTransform.offsetMax = -Margin;
    }
}