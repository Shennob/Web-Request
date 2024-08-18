using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Source;
using UnityEngine;
using UnityEngine.Networking;

public class EventService : MonoBehaviour
{
    [SerializeField] private string _serverUrl;

    private readonly List<UniqueEvent> _allEvents = new();
    
    public void TrackEvent(string type, string data)
    {
        UniqueEvent newEvent = new UniqueEvent(type, data);

        _allEvents.Add(newEvent);
    }

    private IEnumerator SendRequest()
    {
        ContainerEvents container = new();

        List<UniqueEvent> tempContainer = new();

        foreach (var trackedEvent in _allEvents)
        {
            tempContainer.Add(trackedEvent);
        }

        container.Events = tempContainer.ToArray();

        string json = JsonUtility.ToJson(container);

        UnityWebRequest request = UnityWebRequest.Post(_serverUrl, json);
        {
            yield return request.SendWebRequest();
            {
                if (request.isNetworkError || request.isHttpError)
                {
                    //тут заносим в обратно в лист
                }
                else
                {
                    UniqueEvent[] returnedEvents = JsonUtility.FromJson<UniqueEvent[]>(request.downloadHandler.text);

                    if (returnedEvents != null)
                    {
                        foreach (var @event in returnedEvents)
                        {
                            foreach (var element in tempContainer.Where(element => element.Id == @event.Id))
                                tempContainer.Remove(@event);
                        }
                    }
                }
                // а тут нужно сформировать новый лист из элементов с оставшимися эвентами без состояния
            }
        }
    }
}