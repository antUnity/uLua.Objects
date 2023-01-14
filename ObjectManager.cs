using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

namespace uLua.Objects {
    [MoonSharpHideMember("ExternalDirectory")]
    public abstract class ObjectManager<ManagerType, ObjectBase> : ExposedMonoBehaviour where ObjectBase : LuaMonoBehaviour where ManagerType: class, IHasLuaIndexer {
        // Fields

        protected Dictionary<string, ObjectBase> Objects = new Dictionary<string, ObjectBase>();
        
        protected Dictionary<string, string> Scripts = new Dictionary<string, string>();
        
        public string State = "";

        new public ObjectBase this[string Handle] {
            get { return Objects.ContainsKey(Handle) ? Objects[Handle] : default; }
        }

        new public ObjectBase this[DynValue Handle] {
            get { return this[Handle.ToPrintString()]; }
        }

        // Properties
        // Public

        public string ExternalDirectory {
            get { return API.ExternalDirectory; }
        }

        // Methods
        // Public

        public virtual void Clear(bool Async = true) {
            Objects.Clear();
            Scripts.Clear();

            if (State != "") Lua.Log($"{name}: Cleared: '{State}'.");

            State = "";
        }

        // Protected
        
        protected override void OnDestroy() {
            Objects.Clear();
            Scripts.Clear();

            State = "";

            base.OnDestroy();
        }

        protected void Remove(ObjectBase Object) {
            if (Object) {
                // Unregister children before removing parent object
                UnregisterChildrenOf(Object);

                string ID = Object.Handle;
                DestroyImmediate(Object.gameObject);
                Objects.Remove(ID);
            }
        }

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
    }
}