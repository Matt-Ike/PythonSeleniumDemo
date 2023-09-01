import time
import random
import requests
import pyodbc 
import os
import ssl
import smtplib
from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.common.by import By
from selenium.webdriver.support.select import Select
from selenium.common.exceptions import NoSuchElementException
from email.message import EmailMessage

#region selenium setup
COMMON_TIME_OUT = 3
QUICK_TIME_OUT = 1

def pauseBetweenSteps(sleepSeconds):
    time.sleep(sleepSeconds)

driver = webdriver.Firefox()
driver.get("http://localhost:5134")
#endregion

#region recipeGenerator
API_URL = "https://www.themealdb.com/api/json/v1/1/random.php";

#Connection to the database
conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=(local);'
                      'Database=TastyTreats;'
                      'Integrated Security=SSPI;'
                      'TrustServerCertificate=True;')
cursor = conn.cursor()

#Query to get the list of chefs
getChefsQuery = "SELECT LastName + ' ' + FirstName as FullName FROM Chef"

#Query to get the list of recipe types
getRecipeTypesQuery = "SELECT Name FROM RecipeType"

def generateRandomRecipe():    
    #Initialize lists to hold the chef names and recipe types
    chefs = []
    recipeTypes = []
    
    #Get random title from API
    title = requests.get(API_URL).json()['meals'][0]['strMeal'];
    
    #Get random chef from database
    cursor.execute(getChefsQuery)
    for chef in cursor:
        chefs.append(chef.FullName)
    chefName = random.choice(chefs)
        
    #Get random recipe type from database
    cursor.execute(getRecipeTypesQuery)
    for recipeType in cursor:
        recipeTypes.append(recipeType.Name)
    recipeTypeName = random.choice(recipeTypes)
    
    recipeYield = random.randint(1, 13)
    
    archived = bool(random.randint(0,1))
    
    return {
        "Title" : title,
        "ChefName" : chefName,
        "RecipeTypeName" : recipeTypeName,
        "Yield" : recipeYield,
        "Archived" : archived
    }
#endregion

#region Email Setup
email_sender = ''
email_password = ''
email_receiver = ''
#endregion

try:
    while True:
        
        recipe = generateRandomRecipe()
        print(recipe)
        
        #Navigate to the recipes page
        pauseBetweenSteps(COMMON_TIME_OUT)
        recipesLink = driver.find_element(By.XPATH, "/html/body/header/nav/div/div/ul/li[2]/a")
        recipesLink.click()

        # #Navigate to the add recipe page
        pauseBetweenSteps(COMMON_TIME_OUT)
        createRecipeLink = driver.find_element(By.XPATH, "/html/body/div/main/p/a")
        createRecipeLink.click()

        #Fill out the form to add a recipe

        #Fill out title
        pauseBetweenSteps(COMMON_TIME_OUT)
        titleField = driver.find_element(By.XPATH, '//*[@id="Title"]')
        titleField.clear()
        titleField.send_keys(recipe["Title"])

        #Select Chef
        pauseBetweenSteps(2)
        chefSelect = Select(driver.find_element(By.XPATH, '//*[@id="ChefId"]'))
        chefSelect.select_by_visible_text(recipe["ChefName"])

        #State Yield
        pauseBetweenSteps(2)
        recipeYield = driver.find_element(By.XPATH, '//*[@id="Yield"]')
        recipeYield.clear()
        recipeYield.send_keys(recipe["Yield"])

        #Select Recipe Type
        pauseBetweenSteps(2)
        recipeTypeSelect = Select(driver.find_element(By.XPATH, '//*[@id="RecipeTypeId"]'))
        recipeTypeSelect.select_by_visible_text(recipe["RecipeTypeName"])

        #Toggle Archived
        pauseBetweenSteps(2)
        if(recipe["Archived"]):
            archiveToggle = driver.find_element(By.XPATH, '//*[@id="Archived"]')
            archiveToggle.click()
            
        # #Submit the form
        pauseBetweenSteps(2)
        createRecipeSubmit = driver.find_element(By.XPATH, "/html/body/div/main/div/div/form/div[7]/input")
        createRecipeSubmit.click()
        
        #Check For Errors:
        errorList = [];
        try:
            if driver.find_element(By.XPATH, '//*[@id="Title-error"]'):
                print("ERROR on title")
                errorList.append("Error on attribute: Title")
        except NoSuchElementException:
            print("No error on title")
            
        # try:
        #     if driver.find_element(By.XPATH, '/html/body/div/main/div/div/form/div[1]/ul/li/text()'):
        #         print("ERROR on yield")
        #         errorList.append("Business Error: Pastry Chef can have only desserts")
        # except NoSuchElementException:
        #     print("No error on yield")
            
        try:
            if driver.find_element(By.XPATH, '//*[@id="ChefId-error"]'):
                print("ERROR on chef")
                errorList.append("Error on attribute: Chef Selector")
        except NoSuchElementException:
            print("No error on chef")
                
        try:
            if driver.find_element(By.XPATH, '//*[@id="Yield-error"]'):
                print("ERROR on yield")
                errorList.append("Error on attribute: Yield")
        except NoSuchElementException:
            print("No error on yield")
                
        try:
            if driver.find_element(By.XPATH, '//*[@id="RecipeTypeId-error"]'):
                print("ERROR on recipe type")
                errorList.append("Error on attribute: Recipe Type Selector")
        except NoSuchElementException:
            print("No error on recipe type")

            
        #There are errors! send a email to the user
        if len(errorList) > 0:
            print("Errors found! Sending email!")
            driver.save_full_page_screenshot("error.png")
            
            subject = f"{len(errorList)} Errors on Recipe Creation!"
            body1 = f"""
            The following errors were found on the recipe creation page:
            """
            
            formattedList = '\r\t\t'.join(errorList)
            
            body2 = f"""
            \t{formattedList}
            \r
            The Data to be submitted:\r\n
            \tTitle: {recipe["Title"]}\r
            \tChef: {recipe["ChefName"]}\r
            \tYield: {recipe["Yield"]}\r
            \tRecipe Type: {recipe["RecipeTypeName"]}\r
            \tArchived: {recipe["Archived"]}\r\n
            
            Please find an attached a screenshot of the page with the errors.
            """
            
            em = EmailMessage()
            em['From'] = email_sender
            em['To'] = email_receiver
            em['Subject'] = subject
            em.set_content(body1+body2)
            
            with open("error.png", 'rb') as errorImg:
                img = errorImg.read()
                em.add_attachment(img, content_manager=None, maintype="image", subtype = 'png')
                
            pauseBetweenSteps(5)
            os.remove("error.png")
            
            context = ssl.create_default_context()
            
            with smtplib.SMTP_SSL('smtp.gmail.com', 465, context = context) as server:
                server.login(email_sender, email_password)
                server.send_message(em)
        
except KeyboardInterrupt as err:
    print("Keyboard Interrupt...Stopping Program")