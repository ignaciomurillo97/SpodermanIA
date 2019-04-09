using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using UnityEngine.UI;
using System.Threading;
using UnityEditor;
using SpeechLib;
using System.Xml;
using System.IO;

public class DictationScript : MonoBehaviour
{
    public GridNodes Grid;
    public AStar algorithm;

    //Keywords codes
    //1 - gridX number
    //2 - gridY number
    //3 - starting Position X
    //4 - starting Position Y
    //5 - starting Position X
    //6 - starting Position Y
    //7 - building Size value
    private ArrayList inputs;
    private DictationRecognizer dictationRecognizer;
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    private SpVoice voice;
    private int recognizedWordValue;
    private int gridX;
    private int gridY;
    private int positionX;
    private int positionY;
    private int targetX;
    private int targetY;
    private int buildingSize;
    private float obstacleRatio;
    private bool diagonalsAllowed;
    private bool VoiceCommandRecognized;
    private String invalidEntry = "Invalid Entry, please try again";

    public GameObject spiderman;

    void Awake()
    {
        Grid = GetComponent<GridNodes>();
        algorithm = GetComponent<AStar>();
        VoiceCommandRecognized = false;
    }

    // Use this for initialization
    void Start()
    {
        voice = new SpVoice();
        voice.Voice = voice.GetVoices().Item(1);
        voice.Volume = 100; // Volume (no xml)
        voice.Rate = 0;  // Rate (no xml)
        actions.Add("Set Grid Width", MatrixX);
        actions.Add("Set Grid Lenght", MatrixY);
        actions.Add("Set Position X", PositionX);
        actions.Add("Set Position Y", PositionY);
        actions.Add("Set Target X", TargetX);
        actions.Add("Set Target Y", TargetY);
        actions.Add("Set Building Size", BuildingSize);
        actions.Add("Set Obstacle Percentage", ObstaclePercetage);
        actions.Add("Set Diagonals On", DiagonalsOn);
        actions.Add("Set Diagonals Off", DiagonalsOff);
        actions.Add("Play", Play);
        actions.Add("Print", Print);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.InitialSilenceTimeoutSeconds = 20;  
        dictationRecognizer.AutoSilenceTimeoutSeconds = 1;  
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;   
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        UnityEngine.Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    public void PlayDebug(){
        List<Node> path = algorithm.ExecuteAlgorithm();
        spiderman.GetComponent<SpidermanController>().followPath(path);
        Grid.InitData();
    }

    private void Play()
    {
        EditorApplication.Beep();
        UnityEngine.Debug.Log("Play");
        algorithm.ExecuteAlgorithm();
        if (Grid.FinalPath.Count > 0){
            spiderman.GetComponent<SpidermanController>().followPath(Grid.FinalPath);
            voice.Speak("Executing algorithm", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        }else{
            voice.Speak("There ir not a valid solution");        
        }
    }

    private void MatrixX()
    {
        recognizedWordValue = 1;
        EditorApplication.Beep();
        keywordRecognizer.Stop();
        PhraseRecognitionSystem.Shutdown();
        voice.Speak("Please enter the grid width value", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        dictationRecognizer.Start();
    }

    private void MatrixY()
    {
        recognizedWordValue = 2;
        EditorApplication.Beep();
        keywordRecognizer.Stop();
        PhraseRecognitionSystem.Shutdown();
        voice.Speak("Please enter the grid lenght value", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        dictationRecognizer.Start();
    }

    private void PositionX()
    {
        recognizedWordValue = 3;
        EditorApplication.Beep();
        keywordRecognizer.Stop();
        PhraseRecognitionSystem.Shutdown();
        voice.Speak("Please enter the position X value", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        dictationRecognizer.Start();
    }

    private void PositionY()
    {
        recognizedWordValue = 4;
        EditorApplication.Beep();
        keywordRecognizer.Stop();
        PhraseRecognitionSystem.Shutdown();
        voice.Speak("Please enter the position Y value", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        dictationRecognizer.Start();
    }

    private void TargetX()
    {
        recognizedWordValue = 5;
        EditorApplication.Beep();
        keywordRecognizer.Stop();
        PhraseRecognitionSystem.Shutdown();
        voice.Speak("Please enter the target X value", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        dictationRecognizer.Start();
    }

    private void TargetY()
    {
        recognizedWordValue = 6;
        EditorApplication.Beep();
        keywordRecognizer.Stop();
        PhraseRecognitionSystem.Shutdown();
        voice.Speak("Please enter the target Y value", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        dictationRecognizer.Start();
    }

    private void BuildingSize()
    {
        recognizedWordValue = 7;
        EditorApplication.Beep();
        keywordRecognizer.Stop();
        PhraseRecognitionSystem.Shutdown();
        voice.Speak("Please enter the building size value", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        dictationRecognizer.Start();
    }

    private void ObstaclePercetage()
    {
        recognizedWordValue = 8;
        EditorApplication.Beep();
        keywordRecognizer.Stop();
        PhraseRecognitionSystem.Shutdown();
        voice.Speak("Please enter the obstacle percentage value", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        dictationRecognizer.Start();
    }

    private void DiagonalsOn()
    {
        EditorApplication.Beep();
        voice.Speak("Diagonals set to on", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        Grid.AllowDiagonals = true;
        Grid.InitData();
    }

    private void DiagonalsOff()
    {
        EditorApplication.Beep();
        voice.Speak("Diagonals set to off", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        Grid.AllowDiagonals = false;
        Grid.InitData();
    }

    private void Print()
    {
        UnityEngine.Debug.Log(this.gridX);
        UnityEngine.Debug.Log(this.gridY);
    }

    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)   //handles recognized text

    {

        VoiceCommandRecognized = true;
        if (recognizedWordValue == 1)
        {
            try
            {
                int result = Int32.Parse(text);
                if(result < 0)
                {
                    voice.Speak("Value must be greater than zero", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                UnityEngine.Debug.Log(text);
                Grid.GridSizeX = result;
                voice.Speak("Grid width set to " + result.ToString(), SpeechVoiceSpeakFlags.SVSFlagsAsync);
                Grid.InitData();
            }
            catch (Exception ex)
            {
                EditorApplication.Beep();
                voice.Speak(invalidEntry, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            finally
            {

            }
        }

        if (recognizedWordValue == 2)
        {
            try
            {
                int result = Int32.Parse(text);
                if (result < 0)
                {
                    voice.Speak("Value must be greater than zero", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                UnityEngine.Debug.Log(text);
                Grid.GridSizeY = result;
                voice.Speak("Grid lenght set to " + result.ToString(), SpeechVoiceSpeakFlags.SVSFlagsAsync);
                Grid.InitData();
            }
            catch (Exception ex)
            {
                EditorApplication.Beep();
                voice.Speak(invalidEntry, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            finally
            {

            }
        }

        if (recognizedWordValue == 3)
        {
            try
            {
                int result = Int32.Parse(text);
                if (result < 0)
                {
                    voice.Speak("Value must be greater than zero", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                if (result >= Grid.GridSizeX)
                {
                    voice.Speak("Value must be lesser than the grid width", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                if (Grid.CheckPosition(result, Grid.StartPositionY))
                {
                    voice.Speak("The Starting position is an obstacle", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                UnityEngine.Debug.Log(text);
                Grid.StartPositionX = result;
                voice.Speak("Starting position X set to " + result.ToString(), SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            catch (Exception ex)
            {
                EditorApplication.Beep();
                voice.Speak(invalidEntry, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            finally
            {

            }
        }

        if (recognizedWordValue == 4)
        {
            try
            {
                int result = Int32.Parse(text);
                if (result < 0)
                {
                    voice.Speak("Value must be greater than zero", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                if (result >= Grid.GridSizeY)
                {
                    voice.Speak("Value must be lesser than the grid lenght", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                if (Grid.CheckPosition(Grid.StartPositionX, result))
                {
                    voice.Speak("The Starting position is an obstacle", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                UnityEngine.Debug.Log(text);
                Grid.StartPositionY = result;
                voice.Speak("Starting position Y set to " + result.ToString(), SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            catch (Exception ex)
            {
                EditorApplication.Beep();
                voice.Speak(invalidEntry, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            finally
            {

            }
        }

        if (recognizedWordValue == 5)
        {
            try
            {
                int result = Int32.Parse(text);
                if (result < 0)
                {
                    voice.Speak("Value must be greater than zero", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                if (result >= Grid.GridSizeX)
                {
                    voice.Speak("Value must be lesser than the grid width", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                if (Grid.CheckPosition(result, Grid.TargetPositionY))
                {
                    voice.Speak("The Starting position is an obstacle", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                UnityEngine.Debug.Log(text);
                Grid.TargetPositionX = result;
                voice.Speak("Target X set to " + result.ToString(), SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            catch (Exception ex)
            {
                EditorApplication.Beep();
                voice.Speak(invalidEntry, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            finally
            {

            }
        }

        if (recognizedWordValue == 6)
        {
            try
            {
                int result = Int32.Parse(text);
                if (result < 0)
                {
                    voice.Speak("Value must be greater than zero", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                if (result >= Grid.GridSizeY)
                {
                    voice.Speak("Value must be lesser than the grid lenght", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                if (Grid.CheckPosition(Grid.TargetPositionX, result))
                {
                    voice.Speak("The Starting position is an obstacle", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                UnityEngine.Debug.Log(text);
                Grid.TargetPositionY = result;
                voice.Speak("Target Y set to " + result.ToString(), SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            catch (Exception ex)
            {
                EditorApplication.Beep();
                voice.Speak(invalidEntry, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            finally
            {

            }
        }

        if (recognizedWordValue == 7)
        {
            try
            {
                int result = Int32.Parse(text);
                if (result < 0)
                {
                    voice.Speak("Value must be greater than zero", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                UnityEngine.Debug.Log(text);
                Grid.NodeSize = result;
                voice.Speak("Building size set to " + result.ToString(), SpeechVoiceSpeakFlags.SVSFlagsAsync);
                Grid.InitData();
            }
            catch (Exception ex)
            {
                EditorApplication.Beep();
                voice.Speak(invalidEntry, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            finally
            {

            }
        }

        if (recognizedWordValue == 8)
        {
            try
            {
                int result = Int32.Parse(text);
                if (result < 0)
                {
                    voice.Speak("Value must be greater than zero", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                if (result > 100)
                {
                    voice.Speak("Value must be lesser one hundred", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    throw new System.ArgumentException();
                }
                UnityEngine.Debug.Log(text);
                Grid.ObstacleProbability = (float)result / 100;
                voice.Speak("Obstacle percentage set to " + result.ToString()+" percent", SpeechVoiceSpeakFlags.SVSFlagsAsync);
                Grid.InitData();
            }
            catch (Exception ex)
            {
                EditorApplication.Beep();
                voice.Speak(invalidEntry, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            finally
            {

            }
        }

    }

    private void DictationRecognizer_DictationHypothesis(string text)
    {
        // 
    }

    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)  //Handles after-recognition
    {
        if (!VoiceCommandRecognized)
        {
            voice.Speak("Time out, please try again");
        }
        dictationRecognizer.Stop();
        PhraseRecognitionSystem.Restart();  //Starts phrase recognition again
        keywordRecognizer.Start();
        
        if (VoiceCommandRecognized)
        {
            VoiceCommandRecognized = false;
        }
    }

    private void DictationRecognizer_DictationError(string error, int hresult){ }

    void OnDestroy() //Destroyer
    {
        dictationRecognizer.DictationResult -= DictationRecognizer_DictationResult;
        dictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
        dictationRecognizer.DictationHypothesis -= DictationRecognizer_DictationHypothesis;
        dictationRecognizer.DictationError -= DictationRecognizer_DictationError;
        dictationRecognizer.Dispose();  
    }

}
