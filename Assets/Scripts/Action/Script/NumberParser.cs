using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Actions.Script {
    public static class NumberParser
    {
        private enum OperationType {
            Plus,
            Minus,
            Multiple,
            Divide,
        }
        private enum TokenType {
            Number,
            Variable
        }
        private class AtomicEquation {
            private double a;
            private double b;
            private OperationType operationType;
            public double execute() {
                switch (operationType) {
                    case OperationType.Plus:
                        return a+b;
                    case OperationType.Minus:
                        return a-b;
                    case OperationType.Multiple:
                        return a*b;
                    case OperationType.Divide:
                        return a/b;
                    default:
                        throw new Exception($"Operation {operationType} has no execution behavior");
                }
            }
        }
    }
}

