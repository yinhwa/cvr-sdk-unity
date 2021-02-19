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
    public class EndSessionBehavior : Behavior<EndSessionBehavior.EntityData>
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

            [DataMember]
            [DisplayName("Execution stages")]
            public BehaviorExecutionStages ExecutionStages { get; set; }
        }

        private class EndSessionProcess : InstantProcess<EntityData>
        {
            private readonly BehaviorExecutionStages executionStages;
            public EndSessionProcess(BehaviorExecutionStages executionStages, EntityData data) : base(data)
            {
                this.executionStages = executionStages;
            }

            /// <inheritdoc />
            public override void Start()
            {
                if ((Data.ExecutionStages & executionStages) > 0)
                {
                    CognitiveVR.CognitiveVR_Manager.Instance.EndSession();
                }
            }
        }

        protected EndSessionBehavior() : this("End Session", BehaviorExecutionStages.Activation)
        {

        }

        public EndSessionBehavior(string name, BehaviorExecutionStages executionStages)
        {
            Data.ExecutionStages = executionStages;
            Data.Name = name;
        }

        public override IProcess GetActivatingProcess()
        {
            return new EndSessionProcess(BehaviorExecutionStages.Activation, Data);
        }

        public override IProcess GetDeactivatingProcess()
        {
            return new EndSessionProcess(BehaviorExecutionStages.Deactivation, Data);
        }
    }
}
#endif