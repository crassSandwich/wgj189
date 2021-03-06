﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using crass;

public class Navigator : Singleton<Navigator>
{
    public bool Navigating { get; private set; }

    public LineRenderer Line;
    public NavigatorCursor Cursor;
    [Tooltip("In world space")]
    public float MinCursorTravelDistanceToAddNewLinePoint;
    public float LineZPosition;
    public BoolVariable OverviewScreenActive;

    Vector2 lastLinePosition => Line.GetPosition(Line.positionCount - 1);

    void Awake ()
    {
        SingletonOverwriteInstance(this);
    }

    void Update ()
    {
        if (OverviewScreenActive.Value)
        {
            if (Input.GetMouseButtonDown(0)) DestroyLine();

            if (Input.GetMouseButton(0)) drawLine();
            else if (Line.positionCount > 1 && !Navigating) startNavigation();
        }
        else if (!Navigating && Line.positionCount > 0) // player tabs away before releasing mouse
        {
            DestroyLine();
        }
    }

    public void DestroyLine ()
    {
        Line.positionCount = 0;
        Navigating = false;
    }

    // for now: just going to start navigation on mouse up, and stop it when the ship gets to the end or the player manually steers away.
        // in the future, could have it so that navigation starts as soon as there's a line that meets some condition (enough points, latest point is far enough away, etc)

    // also going to let you start pressing down outside the ship. as long as you mouse over the ship at some point, a line will be drawn

    // might want to allow a little more tolerance on where the start of the line is too

    void drawLine ()
    {
        if (Line.positionCount == 0)
        {   
            if (Player.Instance.MousedOver) addLinePosition(Cursor.Position);
            return;
        }

        if (Vector2.Distance(lastLinePosition, Cursor.Position) >= MinCursorTravelDistanceToAddNewLinePoint)
        {
            addLinePosition(Cursor.Position);
        }
    }

    void startNavigation ()
    {
        Navigating = true;
    }

    void addLinePosition (Vector2 position)
    {
        var zCorrectedPosition = new Vector3(position.x, position.y, LineZPosition);

        Line.positionCount++;
        Line.SetPosition(Line.positionCount - 1, zCorrectedPosition);
    }
}
