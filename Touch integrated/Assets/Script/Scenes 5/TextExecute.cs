using UnityEngine.UI;
using DG.Tweening;
using System.Text.RegularExpressions; // ����������ʽ����

public class TextExecute : Text
{
    // ��������ʱ�䣬������ Unity �༭��������
    public float duration = 2f;
    public float delay = 1f; // ������ʼǰ���ӳ�ʱ��

    // ������ʼ����ֵ��������
    public void StartValueAnimation(string valueWithUnit)
    {
        // ��ȡ��ֵ�͵�λ
        float targetValue = ExtractValue(valueWithUnit);
        string unit = ExtractUnit(valueWithUnit);

        // ȷ��С��λ��
        int decimalPlaces = DetermineDecimalPlaces(valueWithUnit);

        // ��ǰ��ֵ��ʼΪ 0
        float currentValue = 0f;

        // ʹ�� DOVirtual.DelayedCall ���ӳ�ִ��
        DOVirtual.DelayedCall(delay, () =>
        {
            // ʹ�� DOTween ʵ�ִ� 0 ��Ŀ��ֵ�Ĺ���
            DOTween.To(() => currentValue, x => currentValue = x, targetValue, duration)
                   .OnUpdate(() => UpdateValueDisplay(currentValue, unit, decimalPlaces));
        });
    }

    // ��ȡ�ַ����е���ֵ����
    private float ExtractValue(string valueWithUnit)
    {
        // ʹ��������ʽ��ȡ��ֵ����
        string numberString = Regex.Match(valueWithUnit, @"\d+(\.\d+)?").Value;
        return float.Parse(numberString);
    }

    // ��ȡ�ַ����еĵ�λ����
    private string ExtractUnit(string valueWithUnit)
    {
        // ʹ��������ʽ��ȡ����ֵ����
        string unitString = Regex.Match(valueWithUnit, @"[^\d\.]+").Value.Trim();
        return unitString;
    }

    // ȷ����ֵ��С��λ��
    private int DetermineDecimalPlaces(string valueWithUnit)
    {
        // ʹ��������ʽƥ����ֵ����
        string numberString = Regex.Match(valueWithUnit, @"\d+(\.\d+)?").Value;
        if (numberString.Contains("."))
        {
            return numberString.Split('.')[1].Length; // С������λ��
        }
        return 0; // û��С��λ
    }

    // ���� UI Text �������ʾ����
    private void UpdateValueDisplay(float currentValue, string unit, int decimalPlaces)
    {
        // ��ֵ������ʽ����
        string valueText = $"<size=60><color=#21edff>{currentValue.ToString($"F{decimalPlaces}")}</color></size>";

        // ���ֲ�����ʽ����
        string unitText = $"<size=50><color=#ffffff>{unit}</color></size>";

        this.text = $"{valueText} {unitText}"; // ����С��λ����ʽ����ʾ

    }
}
