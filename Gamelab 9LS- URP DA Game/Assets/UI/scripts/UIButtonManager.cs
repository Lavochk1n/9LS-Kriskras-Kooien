using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class UIButtonManager : MonoBehaviour
{
    public string buttonTag = "uiButton"; // Ensure this tag is defined in Unity
    public float fontSizeIncrease = 4f; // Amount to increase font size by

    void Start()
    {
        // Find all GameObjects with the specified tag
        GameObject[] buttons = GameObject.FindGameObjectsWithTag(buttonTag);

        foreach (GameObject button in buttons)
        {
            // Ensure the UIButtonEffect script is attached
            UIButtonEffect buttonEffects = button.GetComponent<UIButtonEffect>();
            if (buttonEffects == null)
            {
                buttonEffects = button.AddComponent<UIButtonEffect>();
            }

            // Assign the font size increase value
            buttonEffects.fontSizeIncrease = fontSizeIncrease;

            // Add EventTrigger component if not present
            EventTrigger trigger = button.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = button.AddComponent<EventTrigger>();
            }

            // Clear existing triggers to avoid duplicates
            trigger.triggers.Clear();

            // Add necessary events
            AddEvent(trigger, EventTriggerType.PointerEnter, eventData => buttonEffects.OnPointerEnter((PointerEventData)eventData));
            AddEvent(trigger, EventTriggerType.PointerExit, eventData => buttonEffects.OnPointerExit((PointerEventData)eventData));
            AddEvent(trigger, EventTriggerType.Select, eventData => buttonEffects.OnSelect(eventData));
            AddEvent(trigger, EventTriggerType.Deselect, eventData => buttonEffects.OnDeselect(eventData));
        }
    }

    void AddEvent(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }
}
