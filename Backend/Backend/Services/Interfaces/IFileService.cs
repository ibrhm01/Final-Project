﻿using System;
namespace Backend.Services.Interfaces
{
    public interface IFileService
    {
        string ReadFile(string path, string readTemplate);
    }
}

