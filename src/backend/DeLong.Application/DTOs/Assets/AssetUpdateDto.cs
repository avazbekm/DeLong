﻿namespace DeLong.Application.DTOs.Assets;

public class AssetUpdateDto
{
    public long Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
}
