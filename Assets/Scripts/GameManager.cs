using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // House counters
    private int totalRedInHouse, totalGreenInHouse, totalBlueInHouse, totalYellowInHouse;

    // Frames (highlight current player)
    public GameObject frameRed, frameGreen, frameBlue, frameYellow;

    // Selection borders (glow when movable)
    public GameObject redPlayerI_Border, redPlayerII_Border, redPlayerIII_Border, redPlayerIV_Border;
    public GameObject greenPlayerI_Border, greenPlayerII_Border, greenPlayerIII_Border, greenPlayerIV_Border;
    public GameObject bluePlayerI_Border, bluePlayerII_Border, bluePlayerIII_Border, bluePlayerIV_Border;
    public GameObject yellowPlayerI_Border, yellowPlayerII_Border, yellowPlayerIII_Border, yellowPlayerIV_Border;

    // Starting (home) positions
    public Vector3 redPlayerI_Pos, redPlayerII_Pos, redPlayerIII_Pos, redPlayerIV_Pos;
    public Vector3 greenPlayerI_Pos, greenPlayerII_Pos, greenPlayerIII_Pos, greenPlayerIV_Pos;
    public Vector3 bluePlayerI_Pos, bluePlayerII_Pos, bluePlayerIII_Pos, bluePlayerIV_Pos;
    public Vector3 yellowPlayerI_Pos, yellowPlayerII_Pos, yellowPlayerIII_Pos, yellowPlayerIV_Pos;

    // Player buttons
    public Button RedPlayerI_Button, RedPlayerII_Button, RedPlayerIII_Button, RedPlayerIV_Button;
    public Button GreenPlayerI_Button, GreenPlayerII_Button, GreenPlayerIII_Button, GreenPlayerIV_Button;
    public Button BluePlayerI_Button, BluePlayerII_Button, BluePlayerIII_Button, BluePlayerIV_Button;
    public Button YellowPlayerI_Button, YellowPlayerII_Button, YellowPlayerIII_Button, YellowPlayerIV_Button;

    // Win screens & rank texts
    public GameObject blueScreen, greenScreen, redScreen, yellowScreen;
    public Text blueRankText, greenRankText, redRankText, yellowRankText;

    // Current turn
    private string playerTurn = "RED";

    // Dice
    public Transform diceRoll;
    public Button DiceRollButton;
    public Transform redDiceRollPos, greenDiceRollPos, blueDiceRollPos, yellowDiceRollPos;

    // Dice roll result (1–6)
    private int selectDiceNumAnimation;

    // Dice animations
    public GameObject dice1_Roll_Animation;
    public GameObject dice2_Roll_Animation;
    public GameObject dice3_Roll_Animation;
    public GameObject dice4_Roll_Animation;
    public GameObject dice5_Roll_Animation;
    public GameObject dice6_Roll_Animation;

    // Movement paths (each color has its own path)
    public List<GameObject> redMovementBlocks   = new List<GameObject>();
    public List<GameObject> greenMovementBlocks = new List<GameObject>();
    public List<GameObject> blueMovementBlocks  = new List<GameObject>();
    public List<GameObject> yellowMovementBlocks = new List<GameObject>();

    // Player GameObjects
    public GameObject redPlayerI, redPlayerII, redPlayerIII, redPlayerIV;
    public GameObject greenPlayerI, greenPlayerII, greenPlayerIII, greenPlayerIV;
    public GameObject bluePlayerI, bluePlayerII, bluePlayerIII, bluePlayerIV;
    public GameObject yellowPlayerI, yellowPlayerII, yellowPlayerIII, yellowPlayerIV;

    // Steps taken by each piece
    private int redPlayerI_Steps, redPlayerII_Steps, redPlayerIII_Steps, redPlayerIV_Steps;
    private int greenPlayerI_Steps, greenPlayerII_Steps, greenPlayerIII_Steps, greenPlayerIV_Steps;
    private int bluePlayerI_Steps, bluePlayerII_Steps, bluePlayerIII_Steps, bluePlayerIV_Steps;
    private int yellowPlayerI_Steps, yellowPlayerII_Steps, yellowPlayerIII_Steps, yellowPlayerIV_Steps;

    // UI Panels
    public GameObject confirmScreen;
    public GameObject gameCompletedScreen;

    void Start()
    {
        DiceRollButton.interactable = true;

        dice1_Roll_Animation.SetActive(false);
        dice2_Roll_Animation.SetActive(false);
        dice3_Roll_Animation.SetActive(false);
        dice4_Roll_Animation.SetActive(false);
        dice5_Roll_Animation.SetActive(false);
        dice6_Roll_Animation.SetActive(false);

        // Store initial positions (for sending back home)
        redPlayerI_Pos   = redPlayerI.transform.position;
        redPlayerII_Pos  = redPlayerII.transform.position;
        redPlayerIII_Pos = redPlayerIII.transform.position;
        redPlayerIV_Pos  = redPlayerIV.transform.position;

        greenPlayerI_Pos   = greenPlayerI.transform.position;
        greenPlayerII_Pos  = greenPlayerII.transform.position;
        greenPlayerIII_Pos = greenPlayerIII.transform.position;
        greenPlayerIV_Pos  = greenPlayerIV.transform.position;

        bluePlayerI_Pos   = bluePlayerI.transform.position;
        bluePlayerII_Pos  = bluePlayerII.transform.position;
        bluePlayerIII_Pos = bluePlayerIII.transform.position;
        bluePlayerIV_Pos  = bluePlayerIV.transform.position;

        yellowPlayerI_Pos   = yellowPlayerI.transform.position;
        yellowPlayerII_Pos  = yellowPlayerII.transform.position;
        yellowPlayerIII_Pos = yellowPlayerIII.transform.position;
        yellowPlayerIV_Pos  = yellowPlayerIV.transform.position;

        redScreen.SetActive(false);
        greenScreen.SetActive(false);
        blueScreen.SetActive(false);
        yellowScreen.SetActive(false);

        if (MainMenuScript.howManyPlayers == 2)
        {
            bluePlayerI.SetActive(false); bluePlayerII.SetActive(false);
            bluePlayerIII.SetActive(false); bluePlayerIV.SetActive(false);
            yellowPlayerI.SetActive(false); yellowPlayerII.SetActive(false);
            yellowPlayerIII.SetActive(false); yellowPlayerIV.SetActive(false);
        }
        else if (MainMenuScript.howManyPlayers == 3)
        {
            greenPlayerI.SetActive(false); greenPlayerII.SetActive(false);
            greenPlayerIII.SetActive(false); greenPlayerIV.SetActive(false);
        }

        InitializeDice();
    }

    void InitializeDice()
    {
        DiceRollButton.interactable = true;

        dice1_Roll_Animation.SetActive(false);
        dice2_Roll_Animation.SetActive(false);
        dice3_Roll_Animation.SetActive(false);
        dice4_Roll_Animation.SetActive(false);
        dice5_Roll_Animation.SetActive(false);
        dice6_Roll_Animation.SetActive(false);

        CheckWinCondition();

        diceRoll.position = playerTurn switch
        {
            "RED"    => redDiceRollPos.position,
            "GREEN"  => greenDiceRollPos.position,
            "BLUE"   => blueDiceRollPos.position,
            "YELLOW" => yellowDiceRollPos.position,
            _        => diceRoll.position
        };

        frameRed.SetActive(playerTurn == "RED");
        frameGreen.SetActive(playerTurn == "GREEN");
        frameBlue.SetActive(playerTurn == "BLUE");
        frameYellow.SetActive(playerTurn == "YELLOW");

        DisableAllPlayerButtons();
        HideAllBorders();
    }

    void CheckWinCondition()
    {
        if (totalRedInHouse    == 4) { redScreen.SetActive(true);    playerTurn = "NONE"; StartCoroutine(GameOverWait()); return; }
        if (totalGreenInHouse  == 4) { greenScreen.SetActive(true);  playerTurn = "NONE"; StartCoroutine(GameOverWait()); return; }
        if (totalBlueInHouse   == 4) { blueScreen.SetActive(true);   playerTurn = "NONE"; StartCoroutine(GameOverWait()); return; }
        if (totalYellowInHouse == 4) { yellowScreen.SetActive(true); playerTurn = "NONE"; StartCoroutine(GameOverWait()); return; }
    }

    IEnumerator GameOverWait()
    {
        yield return new WaitForSeconds(1.8f);
        gameCompletedScreen.SetActive(true);
    }

    void DisableAllPlayerButtons()
    {
        RedPlayerI_Button.interactable   = false; RedPlayerII_Button.interactable  = false;
        RedPlayerIII_Button.interactable = false; RedPlayerIV_Button.interactable  = false;
        GreenPlayerI_Button.interactable   = false; GreenPlayerII_Button.interactable  = false;
        GreenPlayerIII_Button.interactable = false; GreenPlayerIV_Button.interactable  = false;
        BluePlayerI_Button.interactable   = false; BluePlayerII_Button.interactable  = false;
        BluePlayerIII_Button.interactable = false; BluePlayerIV_Button.interactable  = false;
        YellowPlayerI_Button.interactable   = false; YellowPlayerII_Button.interactable  = false;
        YellowPlayerIII_Button.interactable = false; YellowPlayerIV_Button.interactable  = false;
    }

    void HideAllBorders()
    {
        redPlayerI_Border.SetActive(false);   redPlayerII_Border.SetActive(false);
        redPlayerIII_Border.SetActive(false); redPlayerIV_Border.SetActive(false);
        greenPlayerI_Border.SetActive(false); greenPlayerII_Border.SetActive(false);
        greenPlayerIII_Border.SetActive(false); greenPlayerIV_Border.SetActive(false);
        bluePlayerI_Border.SetActive(false);   bluePlayerII_Border.SetActive(false);
        bluePlayerIII_Border.SetActive(false); bluePlayerIV_Border.SetActive(false);
        yellowPlayerI_Border.SetActive(false); yellowPlayerII_Border.SetActive(false);
        yellowPlayerIII_Border.SetActive(false); yellowPlayerIV_Border.SetActive(false);
    }

    public void DiceRoll()
    {
        SoundManagerScript.diceAudioSource.Play();
        DiceRollButton.interactable = false;

        selectDiceNumAnimation = Random.Range(1, 7);

        dice1_Roll_Animation.SetActive(selectDiceNumAnimation == 1);
        dice2_Roll_Animation.SetActive(selectDiceNumAnimation == 2);
        dice3_Roll_Animation.SetActive(selectDiceNumAnimation == 3);
        dice4_Roll_Animation.SetActive(selectDiceNumAnimation == 4);
        dice5_Roll_Animation.SetActive(selectDiceNumAnimation == 5);
        dice6_Roll_Animation.SetActive(selectDiceNumAnimation == 6);

        StartCoroutine(ShowMovablePiecesAfterDelay(0.7f));
    }

    IEnumerator ShowMovablePiecesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        bool canMove = false;

        if (playerTurn == "RED")
        {
            canMove |= EnableIfPossible(redPlayerI_Steps,   redMovementBlocks.Count, redPlayerI_Border,   RedPlayerI_Button);
            canMove |= EnableIfPossible(redPlayerII_Steps,  redMovementBlocks.Count, redPlayerII_Border,  RedPlayerII_Button);
            canMove |= EnableIfPossible(redPlayerIII_Steps, redMovementBlocks.Count, redPlayerIII_Border, RedPlayerIII_Button);
            canMove |= EnableIfPossible(redPlayerIV_Steps,  redMovementBlocks.Count, redPlayerIV_Border,  RedPlayerIV_Button);

            if (selectDiceNumAnimation == 6)
            {
                if (redPlayerI_Steps   == 0) { redPlayerI_Border.SetActive(true);   RedPlayerI_Button.interactable   = true; canMove = true; }
                if (redPlayerII_Steps  == 0) { redPlayerII_Border.SetActive(true);  RedPlayerII_Button.interactable  = true; canMove = true; }
                if (redPlayerIII_Steps == 0) { redPlayerIII_Border.SetActive(true); RedPlayerIII_Button.interactable = true; canMove = true; }
                if (redPlayerIV_Steps  == 0) { redPlayerIV_Border.SetActive(true);  RedPlayerIV_Button.interactable  = true; canMove = true; }
            }
        }
        else if (playerTurn == "GREEN")
        {
            canMove |= EnableIfPossible(greenPlayerI_Steps,   greenMovementBlocks.Count, greenPlayerI_Border,   GreenPlayerI_Button);
            canMove |= EnableIfPossible(greenPlayerII_Steps,  greenMovementBlocks.Count, greenPlayerII_Border,  GreenPlayerII_Button);
            canMove |= EnableIfPossible(greenPlayerIII_Steps, greenMovementBlocks.Count, greenPlayerIII_Border, GreenPlayerIII_Button);
            canMove |= EnableIfPossible(greenPlayerIV_Steps,  greenMovementBlocks.Count, greenPlayerIV_Border,  GreenPlayerIV_Button);

            if (selectDiceNumAnimation == 6)
            {
                if (greenPlayerI_Steps   == 0) { greenPlayerI_Border.SetActive(true);   GreenPlayerI_Button.interactable   = true; canMove = true; }
                if (greenPlayerII_Steps  == 0) { greenPlayerII_Border.SetActive(true);  GreenPlayerII_Button.interactable  = true; canMove = true; }
                if (greenPlayerIII_Steps == 0) { greenPlayerIII_Border.SetActive(true); GreenPlayerIII_Button.interactable = true; canMove = true; }
                if (greenPlayerIV_Steps  == 0) { greenPlayerIV_Border.SetActive(true);  GreenPlayerIV_Button.interactable  = true; canMove = true; }
            }
        }
        else if (playerTurn == "BLUE")
        {
            canMove |= EnableIfPossible(bluePlayerI_Steps,   blueMovementBlocks.Count, bluePlayerI_Border,   BluePlayerI_Button);
            canMove |= EnableIfPossible(bluePlayerII_Steps,  blueMovementBlocks.Count, bluePlayerII_Border,  BluePlayerII_Button);
            canMove |= EnableIfPossible(bluePlayerIII_Steps, blueMovementBlocks.Count, bluePlayerIII_Border, BluePlayerIII_Button);
            canMove |= EnableIfPossible(bluePlayerIV_Steps,  blueMovementBlocks.Count, bluePlayerIV_Border,  BluePlayerIV_Button);

            if (selectDiceNumAnimation == 6)
            {
                if (bluePlayerI_Steps   == 0) { bluePlayerI_Border.SetActive(true);   BluePlayerI_Button.interactable   = true; canMove = true; }
                if (bluePlayerII_Steps  == 0) { bluePlayerII_Border.SetActive(true);  BluePlayerII_Button.interactable  = true; canMove = true; }
                if (bluePlayerIII_Steps == 0) { bluePlayerIII_Border.SetActive(true); BluePlayerIII_Button.interactable = true; canMove = true; }
                if (bluePlayerIV_Steps  == 0) { bluePlayerIV_Border.SetActive(true);  BluePlayerIV_Button.interactable  = true; canMove = true; }
            }
        }
        else if (playerTurn == "YELLOW")
        {
            canMove |= EnableIfPossible(yellowPlayerI_Steps,   yellowMovementBlocks.Count, yellowPlayerI_Border,   YellowPlayerI_Button);
            canMove |= EnableIfPossible(yellowPlayerII_Steps,  yellowMovementBlocks.Count, yellowPlayerII_Border,  YellowPlayerII_Button);
            canMove |= EnableIfPossible(yellowPlayerIII_Steps, yellowMovementBlocks.Count, yellowPlayerIII_Border, YellowPlayerIII_Button);
            canMove |= EnableIfPossible(yellowPlayerIV_Steps,  yellowMovementBlocks.Count, yellowPlayerIV_Border,  YellowPlayerIV_Button);

            if (selectDiceNumAnimation == 6)
            {
                if (yellowPlayerI_Steps   == 0) { yellowPlayerI_Border.SetActive(true);   YellowPlayerI_Button.interactable   = true; canMove = true; }
                if (yellowPlayerII_Steps  == 0) { yellowPlayerII_Border.SetActive(true);  YellowPlayerII_Button.interactable  = true; canMove = true; }
                if (yellowPlayerIII_Steps == 0) { yellowPlayerIII_Border.SetActive(true); YellowPlayerIII_Button.interactable = true; canMove = true; }
                if (yellowPlayerIV_Steps  == 0) { yellowPlayerIV_Border.SetActive(true);  YellowPlayerIV_Button.interactable  = true; canMove = true; }
            }
        }

        if (!canMove)
        {
            playerTurn = GetNextPlayerTurn();
            InitializeDice();
        }
    }

    bool EnableIfPossible(int steps, int pathLength, GameObject border, Button button)
    {
        if (steps > 0 && pathLength - steps >= selectDiceNumAnimation)
        {
            border.SetActive(true);
            button.interactable = true;
            return true;
        }
        return false;
    }

    string GetNextPlayerTurn()
    {
        if (MainMenuScript.howManyPlayers == 2)
            return playerTurn == "RED" ? "GREEN" : "RED";

        if (MainMenuScript.howManyPlayers == 3)
        {
            if (playerTurn == "RED")    return "BLUE";
            if (playerTurn == "BLUE")   return "YELLOW";
            return "RED";
        }

        // 4 players
        return playerTurn switch
        {
            "RED"    => "BLUE",
            "BLUE"   => "GREEN",
            "GREEN"  => "YELLOW",
            "YELLOW" => "RED",
            _        => "RED"
        };
    }

    // ────────────────────────────────────────────────
    // RED MOVEMENT
    // ────────────────────────────────────────────────

    public void redPlayerI_UI()   { SoundManagerScript.playerAudioSource.Play(); HideAllRedBordersAndDisableButtons();   MovePiece(redPlayerI,   ref redPlayerI_Steps,   redMovementBlocks, "RED",   redPlayerI_Pos);   }
    public void redPlayerII_UI()  { SoundManagerScript.playerAudioSource.Play(); HideAllRedBordersAndDisableButtons();   MovePiece(redPlayerII,  ref redPlayerII_Steps,  redMovementBlocks, "RED",   redPlayerII_Pos);  }
    public void redPlayerIII_UI() { SoundManagerScript.playerAudioSource.Play(); HideAllRedBordersAndDisableButtons();   MovePiece(redPlayerIII, ref redPlayerIII_Steps, redMovementBlocks, "RED",   redPlayerIII_Pos); }
    public void redPlayerIV_UI()  { SoundManagerScript.playerAudioSource.Play(); HideAllRedBordersAndDisableButtons();   MovePiece(redPlayerIV,  ref redPlayerIV_Steps,  redMovementBlocks, "RED",   redPlayerIV_Pos);  }

    void HideAllRedBordersAndDisableButtons()
    {
        redPlayerI_Border.SetActive(false);   RedPlayerI_Button.interactable   = false;
        redPlayerII_Border.SetActive(false);  RedPlayerII_Button.interactable  = false;
        redPlayerIII_Border.SetActive(false); RedPlayerIII_Button.interactable = false;
        redPlayerIV_Border.SetActive(false);  RedPlayerIV_Button.interactable  = false;
    }

    // ────────────────────────────────────────────────
    // GREEN MOVEMENT
    // ────────────────────────────────────────────────

    public void greenPlayerI_UI()   { SoundManagerScript.playerAudioSource.Play(); HideAllGreenBordersAndDisableButtons();   MovePiece(greenPlayerI,   ref greenPlayerI_Steps,   greenMovementBlocks, "GREEN",   greenPlayerI_Pos);   }
    public void greenPlayerII_UI()  { SoundManagerScript.playerAudioSource.Play(); HideAllGreenBordersAndDisableButtons();   MovePiece(greenPlayerII,  ref greenPlayerII_Steps,  greenMovementBlocks, "GREEN",   greenPlayerII_Pos);  }
    public void greenPlayerIII_UI() { SoundManagerScript.playerAudioSource.Play(); HideAllGreenBordersAndDisableButtons();   MovePiece(greenPlayerIII, ref greenPlayerIII_Steps, greenMovementBlocks, "GREEN",   greenPlayerIII_Pos); }
    public void greenPlayerIV_UI()  { SoundManagerScript.playerAudioSource.Play(); HideAllGreenBordersAndDisableButtons();   MovePiece(greenPlayerIV,  ref greenPlayerIV_Steps,  greenMovementBlocks, "GREEN",   greenPlayerIV_Pos);  }

    void HideAllGreenBordersAndDisableButtons()
    {
        greenPlayerI_Border.SetActive(false);   GreenPlayerI_Button.interactable   = false;
        greenPlayerII_Border.SetActive(false);  GreenPlayerII_Button.interactable  = false;
        greenPlayerIII_Border.SetActive(false); GreenPlayerIII_Button.interactable = false;
        greenPlayerIV_Border.SetActive(false);  GreenPlayerIV_Button.interactable  = false;
    }

    // ────────────────────────────────────────────────
    // BLUE MOVEMENT
    // ────────────────────────────────────────────────

    public void bluePlayerI_UI()   { SoundManagerScript.playerAudioSource.Play(); HideAllBlueBordersAndDisableButtons();   MovePiece(bluePlayerI,   ref bluePlayerI_Steps,   blueMovementBlocks, "BLUE",   bluePlayerI_Pos);   }
    public void bluePlayerII_UI()  { SoundManagerScript.playerAudioSource.Play(); HideAllBlueBordersAndDisableButtons();   MovePiece(bluePlayerII,  ref bluePlayerII_Steps,  blueMovementBlocks, "BLUE",   bluePlayerII_Pos);  }
    public void bluePlayerIII_UI() { SoundManagerScript.playerAudioSource.Play(); HideAllBlueBordersAndDisableButtons();   MovePiece(bluePlayerIII, ref bluePlayerIII_Steps, blueMovementBlocks, "BLUE",   bluePlayerIII_Pos); }
    public void bluePlayerIV_UI()  { SoundManagerScript.playerAudioSource.Play(); HideAllBlueBordersAndDisableButtons();   MovePiece(bluePlayerIV,  ref bluePlayerIV_Steps,  blueMovementBlocks, "BLUE",   bluePlayerIV_Pos);  }

    void HideAllBlueBordersAndDisableButtons()
    {
        bluePlayerI_Border.SetActive(false);   BluePlayerI_Button.interactable   = false;
        bluePlayerII_Border.SetActive(false);  BluePlayerII_Button.interactable  = false;
        bluePlayerIII_Border.SetActive(false); BluePlayerIII_Button.interactable = false;
        bluePlayerIV_Border.SetActive(false);  BluePlayerIV_Button.interactable  = false;
    }

    // ────────────────────────────────────────────────
    // YELLOW MOVEMENT
    // ────────────────────────────────────────────────

    public void yellowPlayerI_UI()   { SoundManagerScript.playerAudioSource.Play(); HideAllYellowBordersAndDisableButtons();   MovePiece(yellowPlayerI,   ref yellowPlayerI_Steps,   yellowMovementBlocks, "YELLOW",   yellowPlayerI_Pos);   }
    public void yellowPlayerII_UI()  { SoundManagerScript.playerAudioSource.Play(); HideAllYellowBordersAndDisableButtons();   MovePiece(yellowPlayerII,  ref yellowPlayerII_Steps,  yellowMovementBlocks, "YELLOW",   yellowPlayerII_Pos);  }
    public void yellowPlayerIII_UI() { SoundManagerScript.playerAudioSource.Play(); HideAllYellowBordersAndDisableButtons();   MovePiece(yellowPlayerIII, ref yellowPlayerIII_Steps, yellowMovementBlocks, "YELLOW",   yellowPlayerIII_Pos); }
    public void yellowPlayerIV_UI()  { SoundManagerScript.playerAudioSource.Play(); HideAllYellowBordersAndDisableButtons();   MovePiece(yellowPlayerIV,  ref yellowPlayerIV_Steps,  yellowMovementBlocks, "YELLOW",   yellowPlayerIV_Pos);  }

    void HideAllYellowBordersAndDisableButtons()
    {
        yellowPlayerI_Border.SetActive(false);   YellowPlayerI_Button.interactable   = false;
        yellowPlayerII_Border.SetActive(false);  YellowPlayerII_Button.interactable  = false;
        yellowPlayerIII_Border.SetActive(false); YellowPlayerIII_Button.interactable = false;
        yellowPlayerIV_Border.SetActive(false);  YellowPlayerIV_Button.interactable  = false;
    }

    // ────────────────────────────────────────────────
    // COMMON MOVEMENT + CAPTURE LOGIC
    // ────────────────────────────────────────────────

    private void MovePiece(GameObject piece, ref int steps, List<GameObject> path, string color, Vector3 homePos)
    {
        if (steps + selectDiceNumAnimation > path.Count) return;

        Vector3[] movePath = new Vector3[selectDiceNumAnimation];
        for (int i = 0; i < selectDiceNumAnimation; i++)
        {
            movePath[i] = path[steps + i].transform.position;
        }

        steps += selectDiceNumAnimation;

        bool reachedHome = (steps == path.Count);

        bool keepTurn = (selectDiceNumAnimation == 6 && !reachedHome);
        playerTurn = keepTurn ? color : GetNextPlayerTurn();

        if (reachedHome)
        {
            if (color == "RED")    totalRedInHouse++;
            if (color == "GREEN")  totalGreenInHouse++;
            if (color == "BLUE")   totalBlueInHouse++;
            if (color == "YELLOW") totalYellowInHouse++;

            // Permanently disable button
            if (color == "RED"    && piece == redPlayerI)   { RedPlayerI_Button.enabled = false;   RedPlayerI_Button.interactable   = false; }
            if (color == "RED"    && piece == redPlayerII)  { RedPlayerII_Button.enabled = false;  RedPlayerII_Button.interactable  = false; }
            if (color == "RED"    && piece == redPlayerIII) { RedPlayerIII_Button.enabled = false; RedPlayerIII_Button.interactable = false; }
            if (color == "RED"    && piece == redPlayerIV)  { RedPlayerIV_Button.enabled = false;  RedPlayerIV_Button.interactable  = false; }
            if (color == "GREEN"  && piece == greenPlayerI)   { GreenPlayerI_Button.enabled = false;   GreenPlayerI_Button.interactable   = false; }
            if (color == "GREEN"  && piece == greenPlayerII)  { GreenPlayerII_Button.enabled = false;  GreenPlayerII_Button.interactable  = false; }
            if (color == "GREEN"  && piece == greenPlayerIII) { GreenPlayerIII_Button.enabled = false; GreenPlayerIII_Button.interactable = false; }
            if (color == "GREEN"  && piece == greenPlayerIV)  { GreenPlayerIV_Button.enabled = false;  GreenPlayerIV_Button.interactable  = false; }
            if (color == "BLUE"   && piece == bluePlayerI)   { BluePlayerI_Button.enabled = false;   BluePlayerI_Button.interactable   = false; }
            if (color == "BLUE"   && piece == bluePlayerII)  { BluePlayerII_Button.enabled = false;  BluePlayerII_Button.interactable  = false; }
            if (color == "BLUE"   && piece == bluePlayerIII) { BluePlayerIII_Button.enabled = false; BluePlayerIII_Button.interactable = false; }
            if (color == "BLUE"   && piece == bluePlayerIV)  { BluePlayerIV_Button.enabled = false;  BluePlayerIV_Button.interactable  = false; }
            if (color == "YELLOW" && piece == yellowPlayerI)   { YellowPlayerI_Button.enabled = false;   YellowPlayerI_Button.interactable   = false; }
            if (color == "YELLOW" && piece == yellowPlayerII)  { YellowPlayerII_Button.enabled = false;  YellowPlayerII_Button.interactable  = false; }
            if (color == "YELLOW" && piece == yellowPlayerIII) { YellowPlayerIII_Button.enabled = false; YellowPlayerIII_Button.interactable = false; }
            if (color == "YELLOW" && piece == yellowPlayerIV)  { YellowPlayerIV_Button.enabled = false;  YellowPlayerIV_Button.interactable  = false; }
        }

        iTween.MoveTo(piece,
            iTween.Hash(
                "path", movePath,
                "time", 1.8f,
                "easetype", iTween.EaseType.easeOutExpo,
                "oncomplete", "OnMoveComplete",
                "oncompleteparams", new object[] { piece, color, homePos },
                "oncompletetarget", gameObject
            ));
    }

    public void OnMoveComplete(object[] args)
    {
        GameObject piece = (GameObject)args[0];
        string color = (string)args[1];
        Vector3 homePos = (Vector3)args[2];

        CheckCapture(piece.transform.position, color, homePos);

        InitializeDice();
    }

    // ────────────────────────────────────────────────
    // CAPTURE LOGIC (main part you asked for)
    // ────────────────────────────────────────────────

    private void CheckCapture(Vector3 landedPosition, string movingColor, Vector3 homePos)
    {
        // We skip capture on safe tiles – you should define safe tiles yourself
        // For simplicity we assume every tile is unsafe except home entrance and final path
        // You can add your own safe tile check here (e.g. by name or tag or index)

        bool isSafeTile = false; // ← put your safe tile logic here

        if (isSafeTile) return;

        GameObject[] allOpponents = GetOpponents(movingColor);

        foreach (GameObject opponent in allOpponents)
        {
            if (opponent == null) continue;

            // Same position → capture
            if (Vector3.Distance(opponent.transform.position, landedPosition) < 0.1f)
            {
                string opponentColor = GetColorFromPiece(opponent);

                // Reset steps
                if (opponent == redPlayerI)    redPlayerI_Steps    = 0;
                if (opponent == redPlayerII)   redPlayerII_Steps   = 0;
                if (opponent == redPlayerIII)  redPlayerIII_Steps  = 0;
                if (opponent == redPlayerIV)   redPlayerIV_Steps   = 0;
                if (opponent == greenPlayerI)    greenPlayerI_Steps    = 0;
                if (opponent == greenPlayerII)   greenPlayerII_Steps   = 0;
                if (opponent == greenPlayerIII)  greenPlayerIII_Steps  = 0;
                if (opponent == greenPlayerIV)   greenPlayerIV_Steps   = 0;
                if (opponent == bluePlayerI)    bluePlayerI_Steps    = 0;
                if (opponent == bluePlayerII)   bluePlayerII_Steps   = 0;
                if (opponent == bluePlayerIII)  bluePlayerIII_Steps  = 0;
                if (opponent == bluePlayerIV)   bluePlayerIV_Steps   = 0;
                if (opponent == yellowPlayerI)    yellowPlayerI_Steps    = 0;
                if (opponent == yellowPlayerII)   yellowPlayerII_Steps   = 0;
                if (opponent == yellowPlayerIII)  yellowPlayerIII_Steps  = 0;
                if (opponent == yellowPlayerIV)   yellowPlayerIV_Steps   = 0;

                // Send back home with animation
                iTween.MoveTo(opponent,
                    iTween.Hash(
                        "position", homePos,
                        "time", 0.8f,
                        "easetype", iTween.EaseType.easeInOutQuad
                    ));

                SoundManagerScript.instance.PlayOneShot(SoundManagerScript.instance.captureAudio); // assuming you have this

                break; // only one piece can be captured at a time in classic Ludo
            }
        }
    }

    private GameObject[] GetOpponents(string myColor)
    {
        List<GameObject> opponents = new List<GameObject>();

        if (myColor != "RED")
        {
            opponents.Add(redPlayerI); opponents.Add(redPlayerII);
            opponents.Add(redPlayerIII); opponents.Add(redPlayerIV);
        }
        if (myColor != "GREEN")
        {
            opponents.Add(greenPlayerI); opponents.Add(greenPlayerII);
            opponents.Add(greenPlayerIII); opponents.Add(greenPlayerIV);
        }
        if (myColor != "BLUE")
        {
            opponents.Add(bluePlayerI); opponents.Add(bluePlayerII);
            opponents.Add(bluePlayerIII); opponents.Add(bluePlayerIV);
        }
        if (myColor != "YELLOW")
        {
            opponents.Add(yellowPlayerI); opponents.Add(yellowPlayerII);
            opponents.Add(yellowPlayerIII); opponents.Add(yellowPlayerIV);
        }

        return opponents.ToArray();
    }

    private string GetColorFromPiece(GameObject piece)
    {
        if (piece == redPlayerI || piece == redPlayerII || piece == redPlayerIII || piece == redPlayerIV) return "RED";
        if (piece == greenPlayerI || piece == greenPlayerII || piece == greenPlayerIII || piece == greenPlayerIV) return "GREEN";
        if (piece == bluePlayerI || piece == bluePlayerII || piece == bluePlayerIII || piece == bluePlayerIV) return "BLUE";
        if (piece == yellowPlayerI || piece == yellowPlayerII || piece == yellowPlayerIII || piece == yellowPlayerIV) return "YELLOW";
        return "UNKNOWN";
    }

    // ────────────────────────────────────────────────
    // UI / EXIT METHODS
    // ────────────────────────────────────────────────

    public void yesGameCompleted() { SceneManager.LoadScene("Ludo"); }
    public void noGameCompleted()  { SceneManager.LoadScene("MainMenu"); }
    public void yesMethod()        { SceneManager.LoadScene("MainMenu"); }
    public void noMethod()         { confirmScreen.SetActive(false); }
    public void ExitMethod()       { confirmScreen.SetActive(true); }

    void Update() { }
}
