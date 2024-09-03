using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Actions.Script.Description {
    public enum ParseStage {
        PreSelect,
        Selection,
        PostSelect
    }
    public static class ParseStageUtils {
        public static List<ActionTargetType> getTargetTypes(ParseStage parseStage) {
            switch (parseStage) {
                case ParseStage.PreSelect:
                    return new List<ActionTargetType>{
                        ActionTargetType.Self
                    };
                case ParseStage.Selection:
                    return new List<ActionTargetType>{
                        ActionTargetType.Self,
                        ActionTargetType.Target
                    };
                case ParseStage.PostSelect:
                    return new List<ActionTargetType>{
                        ActionTargetType.Self
                    };
                default:
                    throw new Exception($"{parseStage} was not covered by 'getTargetTypes'");
            }
        }
    }
}
