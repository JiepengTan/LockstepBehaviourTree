﻿using System;

namespace Lockstep.AI {
    //---------------------------------------------------------------
    public abstract unsafe partial class BTPrecondition : BTNode {
        public BTPrecondition(int maxChildCount)
            : base(maxChildCount){ }

        public abstract bool IsTrue( /*in*/ BTWorkingData wData);
    }

    public abstract class BtPreconditionLeaf : BTPrecondition {
        public BtPreconditionLeaf()
            : base(0){ }
    }

    public abstract class BtPreconditionUnary : BTPrecondition { 
        public BtPreconditionUnary()
            : base(1){
        }
        public BtPreconditionUnary(BTPrecondition lhs)
            : base(1){
            AddChild(lhs);
        }
    }

    public abstract class BtPreconditionBinary : BTPrecondition {  
        public BtPreconditionBinary()
            : base(2){
        }
        public BtPreconditionBinary(BTPrecondition lhs, BTPrecondition rhs)
            : base(2){
            AddChild(lhs).AddChild(rhs);
        }
    }

    //--------------------------------------------------------------
    //basic precondition
    [Serializable,GraphProcessor.NodeMenuItem("Precondition/True")]
    public class BtPreconditionTrue : BtPreconditionLeaf {
        public override bool IsTrue( /*in*/ BTWorkingData wData){
            return true;
        }
    }

    [Serializable,GraphProcessor.NodeMenuItem("Precondition/False")]
    public class BtPreconditionFalse : BtPreconditionLeaf {
        public override bool IsTrue( /*in*/ BTWorkingData wData){
            return false;
        }
    }

    //---------------------------------------------------------------
    
    [Serializable,GraphProcessor.NodeMenuItem("Precondition/Not")]
    public class BTPreconditionNot : BtPreconditionUnary {
        public BTPreconditionNot(){ }

        public BTPreconditionNot(BTPrecondition lhs, int uniqueKey)
            : base(lhs){
            _uniqueKey = uniqueKey;
        }

        public override bool IsTrue( /*in*/ BTWorkingData wData){
            return !GetChild<BTPrecondition>(0).IsTrue(wData);
        }
    }

    //---------------------------------------------------------------
    //binary precondition
    [Serializable,GraphProcessor.NodeMenuItem("Precondition/And")]
    public class BtPreconditionAnd : BtPreconditionBinary {
        public BtPreconditionAnd(){ }
        public BtPreconditionAnd(BTPrecondition lhs, BTPrecondition rhs, int uniqueKey)
            : base(lhs, rhs){
            _uniqueKey = uniqueKey;
        }

        public override bool IsTrue( /*in*/ BTWorkingData wData){
            return GetChild<BTPrecondition>(0).IsTrue(wData) &&
                   GetChild<BTPrecondition>(1).IsTrue(wData);
        }
    }

    [Serializable,GraphProcessor.NodeMenuItem("Precondition/Or")]
    public class BtPreconditionOr : BtPreconditionBinary {
        public BtPreconditionOr(){ }
        public BtPreconditionOr(BTPrecondition lhs, BTPrecondition rhs, int uniqueKey)
            : base(lhs, rhs){
            _uniqueKey = uniqueKey;
        }

        public override bool IsTrue( /*in*/ BTWorkingData wData){
            return GetChild<BTPrecondition>(0).IsTrue(wData) ||
                   GetChild<BTPrecondition>(1).IsTrue(wData);
        }
    }

    [Serializable,GraphProcessor.NodeMenuItem("Precondition/Xor")]
    public class BtPreconditionXor : BtPreconditionBinary {
        public BtPreconditionXor(){ }
        public BtPreconditionXor(BTPrecondition lhs, BTPrecondition rhs, int uniqueKey)
            : base(lhs, rhs){
            _uniqueKey = uniqueKey;
        }

        public override bool IsTrue( /*in*/ BTWorkingData wData){
            return GetChild<BTPrecondition>(0).IsTrue(wData) ^
                   GetChild<BTPrecondition>(1).IsTrue(wData);
        }
    }
}