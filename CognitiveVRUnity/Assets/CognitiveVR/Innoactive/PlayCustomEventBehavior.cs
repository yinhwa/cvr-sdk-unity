using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
#if CVR_INNOACTIVE
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.Utils;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.SceneObjects;

namespace Innoactive.Creator.Core.Behaviors
{
    /// <summary>
    /// A behavior that plays audio.
    /// </summary>
    [DataContract(IsReference = true)]
    public class PlayCustomEventBehavior : Behavior<PlayCustomEventBehavior.EntityData>
    {
        /// <summary>
        /// The "play audio" behavior's data.
        /// </summary>
        [DisplayName("Cognitive3D Event")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            /// <inheritdoc />
            public Metadata Metadata { get; set; }

            /// <inheritdoc />
            public string Name { get; set; }

            //[DataMember]
            //public string CustomEventName { get; set; }

            [DataMember]
            [DisplayName("Event Name")]
            public string EventName { get; set; }

            [DataMember]
            [DisplayName("Dynamic Object (optional)")]
            public SceneObjectReference Target { get; set; }

            [DataMember]
            [DisplayName("Execution stages")]
            public BehaviorExecutionStages ExecutionStages { get; set; }

            //public bool HasSentMessage = false;



            //CONSIDER how could dynamic properties be included here?

            //CONSIDER start event at beginning. send event when step complete?
            //will this be calculated automatically from objectives?
        }

        private class PlayCustomEventProcess : InstantProcess<EntityData>
        {
            private readonly BehaviorExecutionStages executionStages;
            public PlayCustomEventProcess(BehaviorExecutionStages executionStages, EntityData data) : base(data)
            {
                this.executionStages = executionStages;
            }

            /// <inheritdoc />
            public override void Start()
            {
                if ((Data.ExecutionStages & executionStages) > 0)
                {
                    Debug.Log("START: execution stages " + Data.ExecutionStages + "    " + executionStages);
                    SendEvent();
                }
            }

            void SendEvent()
            {
                Debug.Log("c3d send event " + Data.EventName);
                Vector3 eventPosition = CognitiveVR.GameplayReferences.HMD.position;

                string dynamicId = "";
                CognitiveVR.DynamicObject dynamic;
                if (Data.Target != null)
                {
                    if (Data.Target.Value != null)
                    {
                        if (Data.Target.Value.GameObject != null)
                        {
                            dynamic = Data.Target.Value.GameObject.GetComponent<CognitiveVR.DynamicObject>();
                            dynamicId = dynamic.GetId();
                            eventPosition = dynamic.transform.position;
                        }
                    }
                }

                CognitiveVR.CustomEvent.SendCustomEvent(Data.EventName, eventPosition, dynamicId);
            }
        }

        protected PlayCustomEventBehavior() : this("Record Event","New Event", "", BehaviorExecutionStages.Deactivation)
        {
        }

        public PlayCustomEventBehavior(string name, string eventName, ISceneObject targetObject, BehaviorExecutionStages executionStages) : this(name, eventName, TrainingReferenceUtils.GetNameFrom(targetObject),executionStages)
        {
            
        }

        public PlayCustomEventBehavior(string name, string eventName, string targetObject, BehaviorExecutionStages executionStages)
        {
            Data.Target = new SceneObjectReference(targetObject);
            Data.EventName = eventName;
            Data.Name = name;
            Data.ExecutionStages = executionStages;
        }

        /// <inheritdoc />
        public override IProcess GetActivatingProcess()
        {
            return new PlayCustomEventProcess(BehaviorExecutionStages.Activation, Data);
        }

        public override IProcess GetDeactivatingProcess()
        {
            return new PlayCustomEventProcess(BehaviorExecutionStages.Deactivation, Data);
        }
    }
}
#endif