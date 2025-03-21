using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.BehaviourTrees
{

    [Name("Merge", -1)]
    [Description("Merge can accept multiple input connections and thus possible to re-use branches from multiple parents.")]
    [Category("Decorators")]
    [ParadoxNotion.Design.Icon("Merge")]
    public class Merge : BTDecorator
    {

        public override int maxInConnections => -1;

        protected override Status OnExecute(Component agent, IBlackboard blackboard) {
            if ( status != Status.Running ) { decoratedConnection.Reset(); }
            return decoratedConnection.Execute(agent, blackboard);
        }
    }
}