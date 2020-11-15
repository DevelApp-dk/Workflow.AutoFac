using DevelApp.RuntimePluggableClassFactory;
using DevelApp.Workflow.Core;
using DevelApp.Workflow.Core.Exceptions;
using DevelApp.Workflow.Core.Model;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelApp.Workflow.Factories
{
    //TODO Move to Worflow Project if standard library is given up as it requires core 3.1 to work

    /// <summary>
    /// Holds the factory for SagaStepBehavior coming from plugin directory
    /// </summary>
    public class SagaStepBehaviorFactory : ISagaStepBehaviorFactory
    {
        private PluginClassFactory<ISagaStepBehavior> _pluginSagaStepBehaviorFactory;
        public SagaStepBehaviorFactory(Uri pluginPathUri, int retainOldVersions)
        {
            try
            {
                _pluginSagaStepBehaviorFactory = new PluginClassFactory<ISagaStepBehavior>(retainOldVersions);
                _pluginSagaStepBehaviorFactory.LoadFromDirectory(pluginPathUri);
            }
            catch (Exception ex)
            {
                throw new WorkflowStartupException($"Error occured when loading SagaStepBehaviors from {pluginPathUri.LocalPath}", ex);
            }
        }

        /// <summary>
        /// Returns the SagaStepBehavior requested and null if 
        /// </summary>
        /// <param name="behaviorName"></param>
        /// <param name="version"></param>
        /// <param name="behaviorConfiguration"></param>
        /// <returns></returns>
        public ISagaStepBehavior GetSagaStepBehavior(KeyString behaviorName, VersionNumber version, JsonValue behaviorConfiguration)
        {
            ISagaStepBehavior sagaStepBehavior = _pluginSagaStepBehaviorFactory.GetInstance(behaviorName, version);
            if(sagaStepBehavior != null)
            {
                if(!sagaStepBehavior.SetBehaviorConfiguration(behaviorConfiguration))
                {
                    throw new WorkflowRuntimeException($"The supplied behaviorConfiguration is not valid");
                }
            }
            return sagaStepBehavior;
        }
    }
}
