using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TypeMessage
{
    Always,
    OneTime,
    TimedMessage
}

public class MessageHandler : MonoBehaviour
{
    // Parameters
    [SerializeField] public string MessageText = string.Empty;

    // Cached
    private Canvas boxCanvas;
	private TMP_Text tmp_MessageText;
    private Image panelBackground;
    private float time = 0f;
    private float nextTimer = 0f;

    // State
    private TypeMessage currentMessage = TypeMessage.Always;

    // Start is called before the first frame update
    private void Awake(){
        boxCanvas = this.GetComponent<Canvas>();
        tmp_MessageText = this.GetComponentInChildren<TMP_Text>();
        panelBackground = this.GetComponentInChildren<Image>();

        tmp_MessageText.text = MessageText;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        CheckForTimedMessage();
    }

    #region Toggle Canvas

    /// <summary>
    /// Toggles Canvas On/Off based on input
    /// </summary>
    /// <param name="isEnabled"></param>
    public void ToggleCanvas(bool isEnabled)
    {
        boxCanvas.enabled = isEnabled;
    }

    #endregion

    #region Set Message

    /// <summary>
    /// Changes the text of the box message
    /// </summary>
    /// <param name="strMessage">New message to be inserted</param>
    public void SetMessage(string strMessage)
    {
        tmp_MessageText.text = strMessage;
    }

    /// <summary>
    /// Changes Text of the Message and makes it a timed message
    /// </summary>
    /// <param name="strMessage"></param>
    /// <param name="f_messageDuration"></param>
    public void SetMessage(string strMessage, float f_messageDuration)
    {
        ToggleCanvas(true);
        SetMessage(strMessage);
        nextTimer = time + f_messageDuration;
        currentMessage = TypeMessage.TimedMessage;
    }

    #endregion

    #region Change Font Color

    /// <summary>
    /// Changes the Font folor of the text
    /// </summary>
    /// <param name="newColor">New color</param>
    public void ChangeFontColor(Color newColor)
    {
        tmp_MessageText.color = newColor;
    }

    #endregion

    #region Change Background Color

    /// <summary>
    /// Change background color of the panel
    /// </summary>
    /// <param name="newBackgroundColor">New Color for background</param>
    public void ChangeBackgroundColor(Color newBackgroundColor)
    {
        panelBackground.color = newBackgroundColor;
    }

    #endregion

    #region Check For Timed Message

    /// <summary>
    /// Checks if 
    /// </summary>
    private void CheckForTimedMessage()
    {
        if (currentMessage == TypeMessage.TimedMessage)
        {
            if (time >= nextTimer)
            {
                ToggleCanvas(false);
                currentMessage = TypeMessage.Always;
            }
        }
    }

    #endregion

    #region Return Type Message

    /// <summary>
    /// Returns the current type message configured
    /// </summary>
    /// <returns></returns>
    public TypeMessage ReturnTypeMessage() 
    {
        return currentMessage;
    }

    #endregion

}
