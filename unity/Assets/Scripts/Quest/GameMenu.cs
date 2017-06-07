﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.Content;
using Assets.Scripts.UI.Screens;
using Assets.Scripts.UI;
using ValkyrieTools;

// In quest game menu
public class GameMenu {
    private static readonly StringKey SAVE = new StringKey("val","SAVE");
    private static readonly StringKey MAIN_MENU = new StringKey("val", "MAIN_MENU");
    private static readonly StringKey UNDO = new StringKey("val", "UNDO");

    // Open the menu
    public static void Create()
    {
        Game game = Game.Get();
        if (GameObject.FindGameObjectWithTag(Game.DIALOG) != null)
        {
            return;
        }

        if (GameObject.FindGameObjectWithTag(Game.ACTIVATION) != null)
        {
            return;
        }

        // Take screen shot for save before menu is drawn
        game.cc.TakeScreenshot(delegate { Draw(); });
    }

    public static void Draw()
    {
        Game game = Game.Get();
        // Border around menu items
        UIElement ui = new UIElement();
        ui.SetLocation((UIScaler.GetWidthUnits() - 12) / 2, 6, 12, 13);
        new UIElementBorder(ui);

        ui = new UIElement();
        ui.SetLocation((UIScaler.GetWidthUnits() - 10) / 2, 7, 10, 2);
        ui.SetText(UNDO);
        ui.SetBGColor(new Color(0.03f, 0.0f, 0f));
        ui.SetFontSize(UIScaler.GetMediumFont());
        ui.SetFont(game.gameType.GetHeaderFont());
        ui.SetButton(delegate { Undo(); });

        ui = new UIElement();
        ui.SetLocation((UIScaler.GetWidthUnits() - 10) / 2, 10, 10, 2);
        ui.SetText(SAVE);
        ui.SetBGColor(new Color(0.03f, 0.0f, 0f));
        ui.SetFontSize(UIScaler.GetMediumFont());
        ui.SetFont(game.gameType.GetHeaderFont());
        ui.SetButton(delegate { Save(); });

        ui = new UIElement();
        ui.SetLocation((UIScaler.GetWidthUnits() - 10) / 2, 13, 10, 2);
        if (game.testMode)
        {
            ui.SetText(new StringKey("val", "EDITOR"));
            ui.SetButton(delegate { Editor(); });
        }
        else
        {
            ui.SetText(MAIN_MENU);
            ui.SetButton(delegate { Quit(); });
        }
        ui.SetBGColor(new Color(0.03f, 0.0f, 0f));
        ui.SetFontSize(UIScaler.GetMediumFont());
        ui.SetFont(game.gameType.GetHeaderFont());

        ui = new UIElement();
        ui.SetLocation((UIScaler.GetWidthUnits() - 10) / 2, 16, 10, 2);
        ui.SetText(CommonStringKeys.CANCEL);
        ui.SetBGColor(new Color(0.03f, 0.0f, 0f));
        ui.SetFontSize(UIScaler.GetMediumFont());
        ui.SetFont(game.gameType.GetHeaderFont());
        ui.SetButton(delegate { Destroyer.Dialog(); });
    }

    public static void Undo()
    {
        Game game = Game.Get();
        Destroyer.Dialog();
        game.quest.Undo();
    }

    public static void Save()
    {
        Destroyer.Dialog();
        new SaveSelectScreen(true);
    }

    public static void Quit()
    {
        Destroyer.Dialog();
        SaveManager.Save(0, true);
    }

    public static void Editor()
    {
        Game game = Game.Get();
        QuestData.Quest q = game.quest.qd.quest;
        Destroyer.Destroy();

        game.cd = new ContentData(game.gameType.DataDirectory());
        foreach (string pack in game.cd.GetPacks())
        {
            game.cd.LoadContent(pack);
        }

        // Fetch all of the quest data
        ValkyrieDebug.Log("Selecting Quest: " + q.path + System.Environment.NewLine);
        game.quest = new Quest(q);
        ValkyrieDebug.Log("Starting Editor" + System.Environment.NewLine);
        QuestEditor.Begin();
    }
}
