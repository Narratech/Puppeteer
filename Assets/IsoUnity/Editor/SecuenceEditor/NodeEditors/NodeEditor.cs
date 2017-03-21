﻿using UnityEngine;
using System.Collections;

namespace Isometra.Sequences {
	public interface NodeEditor {
		void draw();
		SequenceNode Result { get; }
		string NodeName{ get; }
		NodeEditor clone();
	    string[] ChildNames { get; }
		bool manages(SequenceNode c);
		void useNode(SequenceNode c);
	}
}