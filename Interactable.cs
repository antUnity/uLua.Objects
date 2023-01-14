using UnityEngine;
using UnityEngine.EventSystems;

namespace uLua.Objects {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class Interactable : MonoBehaviour {
        // Fields

        private RectTransform _RectTransform = null;

        private bool _FadeFinished = false;

        private bool _HasChanged = false;

        private bool _MovementFinished = false;

        private float FadeStep = 0.015f;

        private int FadeDirection = 0;

        private bool Flashing = false;

        public Vector2 Drive = Vector2.zero;

        public Interactable Parent = null;

        [SerializeField] private Rect Rectangle = new(0f, 0f, -1f, -1f);

        [SerializeField] private bool _Interactive = true;

        [Range(0f, 1f)] public float Transparency = 0f;

        // Properties
        // Public

        public bool FadeFinished {
            get {
                if (_FadeFinished) {
                    _FadeFinished = false;
                    return true;
                } else return false;
            }
        }

        public bool HasChanged {
            get {
                if (_HasChanged) {
                    _HasChanged = false;
                    return true;
                } else return false;
            }
            set { _HasChanged = value; }
        }

        public float Height {
            get { return Rectangle.height; }
            set {
                if (value != Rectangle.height) _HasChanged = true;
                Rectangle.height = value;
            }
        }

        public bool Interactive {
            get { return _Interactive; }
            set {
                if (value != _Interactive) _HasChanged = true;
                _Interactive = value;
            }
        }

        public bool IsClicked { get; set; }

        public bool IsFading {
            get {
                bool IsFading = (FadeDirection != 0);
                return Parent ? (IsFading || Parent.IsFading) : IsFading;
            }
        }

        public bool IsFlashing {
            get {
                bool IsFlashing = Flashing;
                return Parent ? (IsFlashing || Parent.IsFlashing) : IsFlashing;
            }
        }

        public bool IsHovered { get; set; }

        public bool IsSelected { get; set; }

        public bool IsShown {
            get { return gameObject.activeInHierarchy; }
        }

        public float InheritedHeight {
            get { return (Height == -1) ? (Parent?Parent.InheritedHeight:(Screen.height)) : Height; }
        }

        public float InheritedWidth {
            get { return (Width == -1) ? (Parent?Parent.InheritedWidth:(Screen.width*600f/Screen.height)) : Width; }
        }

        public float MaxVelocity { get; set; }

        public bool MovementFinished {
            get {
                if (_MovementFinished) {
                    _MovementFinished = false;
                    return true;
                } else return false;
            }
        }

        public Vector2 Position {
            get { return Rectangle.position; }
            set { Rectangle.position = value; }
        }

        public float Width {
            get { return Rectangle.width; }
            set {
                if (value != Rectangle.width) _HasChanged = true;
                Rectangle.width = value;
            }
        }

        public float X {
            get { return Rectangle.x; }
            set {
                if (value != Rectangle.x) _HasChanged = true;
                Rectangle.x = value;
            }
        }

        public float Y {
            get { return Rectangle.y; }
            set {
                if (value != Rectangle.y) _HasChanged = true;
                Rectangle.y = value;
            }
        }

        // Protected

        protected RectTransform RectTransform {
            get {
                if (!_RectTransform) _RectTransform = GetComponent<RectTransform>();
                return _RectTransform;
            }
        }

        // Methods
        // Public

        public void Deselect() {
            IsSelected = false;

            if (EventSystem.current.currentSelectedGameObject == gameObject) {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        public void DropAt(float TargetX, float TargetY) {
            transform.localPosition = new Vector3(TargetX, TargetY, 0f);
            X = TargetX;
            Y = TargetY;
        }

        public bool FadeIn(float Duration = 0.25f) {
            bool FadeIn = !IsFading;

            if (FadeIn) {
                if (Duration == 0f) Duration = 0.25f;
                Flashing = false;
                FadeStep = Time.fixedDeltaTime / Duration;
                FadeDirection = -1;
                Transparency = 1f;
            }

            return FadeIn;
        }

        public bool FadeOut(float Duration = 0.25f) {
            bool FadeOut = !IsFading;

            if (FadeOut) {
                if (Duration == 0f) Duration = 0.25f;
                Flashing = false;
                FadeStep = Time.fixedDeltaTime / Duration;
                FadeDirection = 1;
                Transparency = 0f;
            }

            return FadeOut;
        }

        public void Flash(float Duration = 0.5f) {
            if (Duration == 0f) Duration = 0.5f;
            Flashing = true;
            FadeStep = Time.fixedDeltaTime / Duration;
            if (FadeDirection == 0) FadeDirection = 1; else FadeDirection *= -1;
        }

        public float GlobalX(float Offset = 0f) {
            return RectTransform.CorrectedX() + (Parent ? (RectTransform.anchorMin.x + Offset) * InheritedWidth : 0f);
        }

        public float GlobalY(float Offset = 0f) {
            return 600f - RectTransform.CorrectedY() + (Parent ? (RectTransform.anchorMin.y + (Offset - 1f)) * InheritedHeight : 0f);
        }

        public void Hide() {
            gameObject.SetActive(false);
            IsHovered = false;
            IsClicked = false;
        }

        public void MoveBy(float TargetX, float TargetY) {
            Drive = new Vector2(TargetX, TargetY);
        }

        public void MoveTo(float TargetX, float TargetY) {
            Drive = new Vector2(TargetX, TargetY) - Position;
        }

        public void Select() {
            IsSelected = true;

            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void Toggle() {
            if (IsShown) Hide(); else Show();
        }

        // Private

        private void OnEnable() {
            ProcessTransparency();

            _HasChanged = true;
        }

        private void LateUpdate() {
            ProcessTransparency();
        }

        private void ProcessTransparency() {
            // Inherit Transparency
            bool IsFlashingOrFading = Flashing || FadeDirection != 0;
            bool Inherit = Parent && ((Parent.IsFlashing || Parent.IsFading) || !IsFlashingOrFading);

            if (Inherit) {
                Transparency = Parent.Transparency;
            } else {
                // Flash and Fade process
                if (IsFlashingOrFading) {
                    Transparency += FadeStep * FadeDirection * Time.deltaTime / Time.fixedDeltaTime;

                    if (Flashing) {
                        if (Transparency >= 1f) {
                            FadeDirection = -1;
                        } else if (Transparency <= 0) {
                            FadeDirection = 1;
                        }
                    } else {
                        if (Transparency >= 1f && FadeDirection == 1) {
                            Transparency = 1f;
                            FadeDirection = 0;

                            _FadeFinished = true;
                        } else if (Transparency <= 0f && FadeDirection == -1) {
                            Transparency = 0f;
                            FadeDirection = 0;

                            _FadeFinished = true;
                        }
                    }
                }
            }
        }

        private void Start() {
            _HasChanged = true;
        }

        private void Update() {
            // Process Position
            if (Drive != Vector2.zero) {
                Vector2 TargetPosition = Position + Drive;

                if (Drive.magnitude > MaxVelocity) {
                    float TimeConstant = Time.deltaTime / Time.fixedDeltaTime;
                    float Angle = Mathf.Atan2(Drive.y, Drive.x);
                    Drive = new Vector2(MaxVelocity * Mathf.Cos(Angle) * TimeConstant, MaxVelocity * Mathf.Sin(Angle) * TimeConstant);
                }

                Position += Drive;

                // Update Drive
                Drive = TargetPosition - Position;
            }
        }

        private void OnValidate() {
            if (Width < 0f) Width = -1f;
            if (Height < 0f) Height = -1f;
            _HasChanged = true;
        }
    }
}
