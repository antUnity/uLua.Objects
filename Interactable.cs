using UnityEngine;
using UnityEngine.EventSystems;

namespace uLua.Objects {
    /// <summary>
    /// 
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class Interactable : MonoBehaviour {
        #region Fields

        private RectTransform _RectTransform = null;

        private bool _FadeFinished = false;

        private bool _HasChanged = false;

        private bool _MovementFinished = false;

        private float FadeStep = 0.015f;

        /** <summary></summary> */
        private int FadeDirection = 0;

        /** <summary></summary> */
        private bool Flashing = false;

        /** <summary></summary> */
        public Vector2 Drive = Vector2.zero;

        /** <summary></summary> */
        public Interactable Parent = null;

        /** <summary></summary> */
        [SerializeField] private Rect Rectangle = new(0f, 0f, -1f, -1f);

        [SerializeField] private bool _Interactive = true;

        /** <summary></summary> */
        [Range(0f, 1f)] public float Transparency = 0f;

        #endregion

        #region Properties

        #region Public

        /// <summary>
        /// 
        /// </summary>
        public bool FadeFinished {
            get {
                if (_FadeFinished) {
                    _FadeFinished = false;
                    return true;
                } else return false;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public bool HasChanged {
            get {
                if (_HasChanged) {
                    _HasChanged = false;
                    return true;
                } else return false;
            }
            set { _HasChanged = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Height {
            get { return Rectangle.height; }
            set {
                if (value != Rectangle.height) _HasChanged = true;
                Rectangle.height = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Interactive {
            get { return _Interactive; }
            set {
                if (value != _Interactive) _HasChanged = true;
                _Interactive = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsClicked { get; set; }

        public bool IsFading {
            get {
                bool IsFading = (FadeDirection != 0);
                return Parent ? (IsFading || Parent.IsFading) : IsFading;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFlashing {
            get {
                bool IsFlashing = Flashing;
                return Parent ? (IsFlashing || Parent.IsFlashing) : IsFlashing;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsHovered { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsShown {
            get { return gameObject.activeInHierarchy; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float InheritedHeight {
            get { return (Height == -1) ? (Parent?Parent.InheritedHeight:(Screen.height)) : Height; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float InheritedWidth {
            get { return (Width == -1) ? (Parent?Parent.InheritedWidth:(Screen.width*600f/Screen.height)) : Width; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float MaxVelocity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool MovementFinished {
            get {
                if (_MovementFinished) {
                    _MovementFinished = false;
                    return true;
                } else return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Position {
            get { return Rectangle.position; }
            set { Rectangle.position = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Width {
            get { return Rectangle.width; }
            set {
                if (value != Rectangle.width) _HasChanged = true;
                Rectangle.width = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float X {
            get { return Rectangle.x; }
            set {
                if (value != Rectangle.x) _HasChanged = true;
                Rectangle.x = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Y {
            get { return Rectangle.y; }
            set {
                if (value != Rectangle.y) _HasChanged = true;
                Rectangle.y = value;
            }
        }

        #endregion

        #region Protected

        /// <summary>
        /// 
        /// </summary>
        protected RectTransform RectTransform {
            get {
                if (!_RectTransform) _RectTransform = GetComponent<RectTransform>();
                return _RectTransform;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// 
        /// </summary>
        public void Deselect() {
            IsSelected = false;

            if (EventSystem.current.currentSelectedGameObject == gameObject) {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TargetX"></param>
        /// <param name="TargetY"></param>
        public void DropAt(float TargetX, float TargetY) {
            transform.localPosition = new Vector3(TargetX, TargetY, 0f);
            X = TargetX;
            Y = TargetY;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Duration"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Duration"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Duration"></param>
        public void Flash(float Duration = 0.5f) {
            if (Duration == 0f) Duration = 0.5f;
            Flashing = true;
            FadeStep = Time.fixedDeltaTime / Duration;
            if (FadeDirection == 0) FadeDirection = 1; else FadeDirection *= -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Offset"></param>
        /// <returns></returns>
        public float GlobalX(float Offset = 0f) {
            return RectTransform.CorrectedX() + (Parent ? (RectTransform.anchorMin.x + Offset) * InheritedWidth : 0f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Offset"></param>
        /// <returns></returns>
        public float GlobalY(float Offset = 0f) {
            return 600f - RectTransform.CorrectedY() + (Parent ? (RectTransform.anchorMin.y + (Offset - 1f)) * InheritedHeight : 0f);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Hide() {
            gameObject.SetActive(false);
            IsHovered = false;
            IsClicked = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TargetX"></param>
        /// <param name="TargetY"></param>
        public void MoveBy(float TargetX, float TargetY) {
            Drive = new Vector2(TargetX, TargetY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TargetX"></param>
        /// <param name="TargetY"></param>
        public void MoveTo(float TargetX, float TargetY) {
            Drive = new Vector2(TargetX, TargetY) - Position;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Select() {
            IsSelected = true;

            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Show() {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Toggle() {
            if (IsShown) Hide(); else Show();
        }

        #endregion

        #region Private

        /// <summary>
        /// 
        /// </summary>
        private void OnEnable() {
            ProcessTransparency();

            _HasChanged = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LateUpdate() {
            ProcessTransparency();
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        private void Start() {
            _HasChanged = true;
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        private void OnValidate() {
            if (Width < 0f) Width = -1f;
            if (Height < 0f) Height = -1f;
            _HasChanged = true;
        }

        #endregion

        #endregion
    }
}
