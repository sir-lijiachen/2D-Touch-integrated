using UnityEngine.UI;
using DG.Tweening;
using System.Text.RegularExpressions; // 用于正则表达式处理

public class TextExecute : Text
{
    // 动画持续时间，可以在 Unity 编辑器中设置
    public float duration = 2f;
    public float delay = 1f; // 动画开始前的延迟时间

    // 方法初始化数值增长动画
    public void StartValueAnimation(string valueWithUnit)
    {
        // 提取数值和单位
        float targetValue = ExtractValue(valueWithUnit);
        string unit = ExtractUnit(valueWithUnit);

        // 确定小数位数
        int decimalPlaces = DetermineDecimalPlaces(valueWithUnit);

        // 当前数值初始为 0
        float currentValue = 0f;

        // 使用 DOVirtual.DelayedCall 来延迟执行
        DOVirtual.DelayedCall(delay, () =>
        {
            // 使用 DOTween 实现从 0 到目标值的过渡
            DOTween.To(() => currentValue, x => currentValue = x, targetValue, duration)
                   .OnUpdate(() => UpdateValueDisplay(currentValue, unit, decimalPlaces));
        });
    }

    // 提取字符串中的数值部分
    private float ExtractValue(string valueWithUnit)
    {
        // 使用正则表达式提取数值部分
        string numberString = Regex.Match(valueWithUnit, @"\d+(\.\d+)?").Value;
        return float.Parse(numberString);
    }

    // 提取字符串中的单位部分
    private string ExtractUnit(string valueWithUnit)
    {
        // 使用正则表达式提取非数值部分
        string unitString = Regex.Match(valueWithUnit, @"[^\d\.]+").Value.Trim();
        return unitString;
    }

    // 确定数值的小数位数
    private int DetermineDecimalPlaces(string valueWithUnit)
    {
        // 使用正则表达式匹配数值部分
        string numberString = Regex.Match(valueWithUnit, @"\d+(\.\d+)?").Value;
        if (numberString.Contains("."))
        {
            return numberString.Split('.')[1].Length; // 小数点后的位数
        }
        return 0; // 没有小数位
    }

    // 更新 UI Text 组件的显示内容
    private void UpdateValueDisplay(float currentValue, string unit, int decimalPlaces)
    {
        // 数值部分样式设置
        string valueText = $"<size=60><color=#21edff>{currentValue.ToString($"F{decimalPlaces}")}</color></size>";

        // 汉字部分样式设置
        string unitText = $"<size=50><color=#ffffff>{unit}</color></size>";

        this.text = $"{valueText} {unitText}"; // 根据小数位数格式化显示

    }
}
