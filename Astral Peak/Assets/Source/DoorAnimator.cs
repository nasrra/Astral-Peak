using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimator : AnimatorOverride{
    public event Action now_opened, now_closed;
    public void open() => animator.Play("Open");
    public void close() => animator.Play("Close");
    public void finished_openening() => now_opened?.Invoke();
    public void finished_closing() => now_closed?.Invoke();
}
