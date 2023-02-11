using UnityEngine;
using MoonSharp.Interpreter;
using UnityEngine.EventSystems;

/// <summary></summary>
namespace uLua.Objects {
    /// <summary>
    /// 
    /// </summary>
    [MoonSharpHideMember("IsParent")]
    [MoonSharpHideMember("Parent")]
    [MoonSharpHideMember("OnPointerClick")]
    [MoonSharpHideMember("OnPointerDown")]
    [MoonSharpHideMember("OnPointerEnter")]
    [MoonSharpHideMember("OnPointerExit")]
    [MoonSharpHideMember("OnSubmit")]
    [MoonSharpHideMember("OnValueChange")]
    [MoonSharpHideMember("OnCancel")]
    [MoonSharpHideMember("OnDeselect")]
    [MoonSharpHideMember("OnSelect")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Interactable))]
    [RequireComponent(typeof(RectTransform))]
    public abstract class Object : ExposedMonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler, ISubmitHandler, ICancelHandler, IDeselectHandler, ISelectHandler {
        #region Fields

        /** <summary></summary> */
        private float TimePassed = 0f;

        /** <summary></summary> */
        private RectTransform _RectTransform = null;

        /** <summary></summary> */
        private Interactable _Interactable = null;

        private string _Value = "";

        /** <summary></summary> */
        [System.NonSerialized] public bool HideOnStart = false;

        #endregion

        #region Properties

        #region Public

        public float Height {
            get { return Interactable.Height; }
            set { Interactable.Height = value; }
        }

        public bool Interactive {
            set { Interactable.Interactive = value; }
        }

        public bool IsClicked {
            get { return Interactable.IsClicked; }
            set { Interactable.IsClicked = value; }
        }

        public bool IsFading {
            get { return Interactable.IsFading; }
        }

        public bool IsFadingIn {
            get { return (IsVisible&&!IsFading)||(IsInvisible&&IsFading); }
        }

        public bool IsFadingOut {
            get { return (IsVisible&&IsFading)||(IsInvisible&&!IsFading); }
        }

        public bool IsFlashing {
            get { return Interactable.IsFlashing; }
        }

        public bool IsHovered {
            get { return Interactable.IsHovered; }
            set { Interactable.IsHovered = value; }
        }

        public bool IsInteractive {
            get { return Interactable.Interactive; }
        }

        public bool IsSelected {
            get { return Interactable.IsSelected; }
            set { Interactable.IsSelected = value; }
        }

        public bool IsShown {
            get { return Interactable.IsShown; }
        }

        public bool IsVisible {
            get { return (Transparency == 0f); }
        }

        public bool IsInvisible {
            get { return (Transparency == 1f); }
        }

        public float MaxVelocity{
            get { return Interactable.MaxVelocity; }
            set { Interactable.MaxVelocity = value; }
        }

        public Interactable Parent {
            set { Interactable.Parent = value; }
            get { return Interactable.Parent; }
        }

        public float Transparency {
            get { return Interactable.Transparency; }
            set { Interactable.Transparency = value; }
        }

        public string Value {
            get { return _Value; }
            set {
                if (_Value != value) {
                    _Value = value;
                    OnValueChange();
                }

                if (value != "") InvokeLua("OnValueSet");
            }
        }

        public float Width {
            get { return Interactable.Width; }
            set { Interactable.Width = value; }
        }

        public float X {
            get { return Interactable.X; }
            set { Interactable.X = value; }
        }

        public float Y {
            get { return Interactable.Y; }
            set { Interactable.Y = value; }
        }

        #endregion

        #region Protected

        protected Interactable Interactable {
            get {
                if (!_Interactable) _Interactable = GetComponent<Interactable>();
                return _Interactable;
            }
        }

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

        public void Deselect() {
            Interactable.Deselect();
        }

        public void DropAt(float TargetX, float TargetY) {
            Interactable.DropAt(TargetX, TargetY);
        }

        public void FadeIn(float Duration = 0.25f) {
            if (IsShown) {
                if (Interactable.FadeIn(Duration)) {
                    InvokeLua("OnFade");
                }
            }
        }

        public void FadeOut(float Duration = 0.25f) {
            if (IsShown) {
                if (Interactable.FadeOut(Duration)) {
                    InvokeLua("OnFade");
                }
            }
        }

        public void Flash(float Duration = 0.25f) {
            Interactable.Flash(Duration);
        }

        public float GlobalX(float Offset = 0f) {
            return Interactable.GlobalX(Offset);
        }

        public float GlobalY(float Offset = 0f) {
            return Interactable.GlobalY(Offset);
        }

        public void Hide() {
            Interactable.Hide();
            HideOnStart = false;
        }

        public void MoveTo(float TargetX, float TargetY) {
            Interactable.MoveTo(TargetX, TargetY);
            InvokeLua("OnMove");
        }

        public virtual void OnCancel(BaseEventData eventData) {
            InvokeLua("OnCancel");
        }

        public virtual void OnDeselect(BaseEventData eventData) {
            InvokeLua("OnDeselect");
            IsSelected = false;
        }

        public virtual void OnPointerClick(PointerEventData eventData) {
            if (IsInteractive) {
                if (eventData.pointerId == -1) { // -1, -2, -3 = left, right, centre
                    IsClicked = false;
                    InvokeLua("OnClick");
                }
            }
        }

        public virtual void OnPointerDown(PointerEventData eventData) {
            if (IsInteractive) {
                IsClicked = true;
                InvokeLua("OnInteract");
                InvokeLua("OnSubmit");
            }
        }

        public virtual void OnPointerEnter(PointerEventData eventData) {
            if (!IsFading) {
                if (IsInteractive && IsExposed) InvokeLua("OnPointerEnter");
                IsHovered = true;
            }
        }

        public virtual void OnPointerExit(PointerEventData eventData) {
            if (IsInteractive && IsExposed) InvokeLua("OnPointerExit");
            IsHovered = false;
            IsClicked = false;
        }

        public virtual void OnSelect(BaseEventData eventData) {
        }

        public virtual void OnSubmit(BaseEventData eventData) {
            if (IsInteractive && !Input.GetKey(KeyCode.RightAlt)) {
                InvokeLua("OnInteract");
                InvokeLua("OnSubmit");
                InvokeLua("OnReturn");
            }
        }

        public virtual void OnValueChange() {
            if (Value == "") {
                InvokeLua("OnValueClear");
            } else InvokeLua("OnValueChange");
        }

        public Type IsParent<Type>() where Type : MonoBehaviour {
            return Parent ? Parent.GetComponent<Type>() : null;
        }

        public void ResizeTo(float Width, float Height) {
            if (this.Width != Width || this.Height != Height) {
                this.Width = Width;
                this.Height = Height;
                if (IsExposed) InvokeLua("OnResize");
            }
        }

        public virtual void Select() {
            Interactable.Select();

            InvokeLua("OnSelect");
        }

        public void Show() {
            Interactable.Show();
            HideOnStart = false;
        }

        public void Toggle() {
            Interactable.Toggle();
        }

        #endregion

        #region Protected

        protected override void Start() {
            base.Start();

            if (HideOnStart) Hide();
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            Destroy(Interactable);
        }

        protected virtual void OnDisable() {
            if (IsExposed) InvokeLua("OnHide");

            Interactable.enabled = false;
        }

        protected virtual void OnEnable() {
            if (IsExposed) InvokeLua("OnShow");

            Interactable.enabled = true;
        }

        protected virtual void OnValidate() {
            Interactable.HasChanged = true;
        }

        protected virtual void Update() {
            // Invoke OnProcess once per second
            TimePassed += Time.deltaTime;
            if (TimePassed >= 1f) {
                if (IsExposed) InvokeLua("OnProcess");
                TimePassed = 0f;
            }

            // Invoke Events
            if (Interactable.FadeFinished) InvokeLua("OnFadeFinish");
            if (Interactable.MovementFinished) InvokeLua("OnStop");
        }

        #endregion

        #endregion
    }
}