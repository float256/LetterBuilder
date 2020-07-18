USE [master]
GO

CREATE DATABASE [DirectoryStructure]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DirectoryStructure', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\DirectoryStructure.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DirectoryStructure_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\DirectoryStructure_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DirectoryStructure].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

