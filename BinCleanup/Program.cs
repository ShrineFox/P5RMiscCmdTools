Thread.Sleep(2000);

foreach(var file in Directory.GetFiles(".\\RepackedBINs\\TEX_WIP.CPK\\MODEL\\FIELD_TEX\\TEXTURES")
    .Where(x => x.EndsWith("_")))
{
    if (File.Exists(file.TrimEnd('_')))
        File.Delete(file.TrimEnd('_'));
}

foreach (var file in Directory.GetFiles(".\\RepackedBINs\\TEX_WIP.CPK\\MODEL\\FIELD_TEX\\TEXTURES")
    .Where(x => x.EndsWith("_")))
{
    File.Move(file, file.TrimEnd('_'));
}