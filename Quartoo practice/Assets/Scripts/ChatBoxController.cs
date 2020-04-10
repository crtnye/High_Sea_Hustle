using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoxController : MonoBehaviour
{
    [SerializeField]
    List<Message> messageList = new List<Message>();

    public GameObject chatPanel;
    public GameObject textObject;
    public InputField messageInputField;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addMessage(string message)
    {
        Message newMessage = new Message();
        newMessage.text = message;

        GameObject newMessageObject = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newMessageObject.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage);
    }

    public void sendMessagePressed()
    {
        string message = messageInputField.text;

        if (message.Length > 0)
            addMessage(message);
    }
}


[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
}
