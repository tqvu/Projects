import unittest
import calculator

class TestCalculator(unittest.TestCase):
    def test_add(self):
        calculator.equation_string = ""
        calculator.button_press("1")
        calculator.button_press("+")
        calculator.button_press("1")
        calculator.equals()
        self.assertEqual(calculator.equation_string, "2")
        
    def test_subtract(self):
        calculator.equation_string = ""
        calculator.button_press("1")
        calculator.button_press("-")
        calculator.button_press("1")
        calculator.equals()
        self.assertEqual(calculator.equation_string, "0")
        
    def test_multiply(self):
        calculator.equation_string = ""
        calculator.button_press("2")
        calculator.button_press("*")
        calculator.button_press("2")
        calculator.equals()
        self.assertEqual(calculator.equation_string, "4")
    
    def test_divide(self):
        calculator.equation_string = ""
        calculator.button_press("2")
        calculator.button_press("/")
        calculator.button_press("2")
        calculator.equals()
        self.assertEqual(calculator.equation_string, "1.0")
    
    def test_divideByZero(self):
        calculator.equation_string = ""
        calculator.button_press("2")
        calculator.button_press("/")
        calculator.button_press("0")
        calculator.equals()
        self.assertEqual(calculator.equation_string, "Error")
    
if __name__ == '__main__':
    unittest.main()
        
        
