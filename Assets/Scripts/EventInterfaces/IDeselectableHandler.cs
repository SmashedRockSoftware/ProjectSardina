using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public interface IDeselectableHandler : IEventSystemHandler {

	void Deselect();

}
