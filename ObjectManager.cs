using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

namespace uLua.Objects {
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ManagerType"></typeparam>
    /// <typeparam name="ObjectBase"></typeparam>
    [MoonSharpHideMember("ExternalDirectory")]
    public abstract class ObjectManager<ManagerType, ObjectBase> : ExposedMonoBehaviour where ObjectBase : LuaMonoBehaviour where ManagerType: class, IHasLuaIndexer {
        #region Fields

        /** <summary></summary> */
        protected Dictionary<string, ObjectBase> Objects = new Dictionary<string, ObjectBase>();

        /** <summary></summary> */
        protected Dictionary<string, string> Scripts = new Dictionary<string, string>();

        /** <summary></summary> */
        public string State = "";

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Handle"></param>
        /// <returns></returns>
        new public ObjectBase this[string Handle] {
            get { return Objects.ContainsKey(Handle) ? Objects[Handle] : default; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Handle"></param>
        /// <returns></returns>
        new public ObjectBase this[DynValue Handle] {
            get { return this[Handle.ToPrintString()]; }
        }

        #region Properties

        #region Public

        public string ExternalDirectory {
            get { return API.ExternalDirectory; }
        }

        #endregion

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Async"></param>
        public virtual void Clear(bool Async = true) {
            Objects.Clear();
            Scripts.Clear();

            if (State != "") Lua.Log($"{name}: Cleared: '{State}'.");

            State = "";
        }

        #endregion

        #region Protected

        /// <summary>
        /// 
        /// </summary>
        protected override void OnDestroy() {
            Objects.Clear();
            Scripts.Clear();

            State = "";

            base.OnDestroy();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Object"></param>
        protected void Remove(ObjectBase Object) {
            if (Object) {
                // Unregister children before removing parent object
                UnregisterChildrenOf(Object);

                string ID = Object.Handle;
                DestroyImmediate(Object.gameObject);
                Objects.Remove(ID);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Object"></param>
        protected void UnregisterChildrenOf(ObjectBase Object) {
            List<ObjectBase> ChildList = new();
            foreach (Transform Child in Object.transform) {
                ObjectBase ChildObject = Child.GetComponent<ObjectBase>();
                if (ChildObject) ChildList.Add(ChildObject);
            }

            foreach (ObjectBase Child in ChildList) {
                UnregisterChildrenOf(Child);
                Objects.Remove(Child.Handle);
            }
        }

        #endregion

        #endregion
    }
}