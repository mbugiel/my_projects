package calc;


import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;

public class Main implements ActionListener {

    private final JFrame frame;
    private final JTextField calcWindow;
    private double number1;
    private String sign = "0";

    private boolean signPressed = false;
    public Main(){


        frame = new JFrame();
        frame.setSize(510, 700);
        frame.setLocationRelativeTo(null);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setResizable(false);

        SpringLayout layout1 = new SpringLayout();
        frame.setLayout(layout1);
        frame.setTitle("Calculator");

        ImageIcon img = new ImageIcon("D:\\images\\ico.png");

        frame.setIconImage(img.getImage());

        JPanel Fieldpanel = new JPanel();
        calcWindow = new JTextField(13);
        calcWindow.setEditable(false);
        calcWindow.setFont(new Font(Font.DIALOG, Font.PLAIN, 36));
        calcWindow.setText("0");
        calcWindow.setHorizontalAlignment(SwingConstants.RIGHT);

        Fieldpanel.add(calcWindow);

        frame.add(Fieldpanel);

        final int north_pad = 30;
        final int west_pad = 30;
        layout1.putConstraint(SpringLayout.NORTH, Fieldpanel, north_pad, SpringLayout.NORTH, frame);
        layout1.putConstraint(SpringLayout.WEST, Fieldpanel, west_pad, SpringLayout.WEST, frame);


        GridLayout grid = new GridLayout(5, 4);
        JPanel buttonpanel = new JPanel();
        buttonpanel.setLayout(grid);


        String label = "0";
        for(int j = 0; j < 20; j++){

            switch(j){
                case 0:
                    label = "C";
                    break;
                case 1:
                    label = "<";
                    break;
                case 2:
                    label = "disable";
                    break;
                case 3:
                    label = "/";
                    break;
                case 4:
                    label = "7";
                    break;
                case 5:
                    label = "8";
                    break;
                case 6:
                    label = "9";
                    break;
                case 7:
                    label = "x";
                    break;
                case 8:
                    label = "4";
                    break;
                case 9:
                    label = "5";
                    break;
                case 10:
                    label = "6";
                    break;
                case 11:
                    label = "-";
                    break;
                case 12:
                    label = "1";
                    break;
                case 13:
                    label = "2";
                    break;
                case 14:
                    label = "3";
                    break;
                case 15:
                    label = "+";
                    break;
                case 16:
                    label = "+/-";
                    break;
                case 17:
                    label = "0";
                    break;
                case 18:
                    label = ".";
                    break;
                case 19:
                    label = "=";
                    break;

            }

            JButton button;

            if(!label.equals("disable")){
                button = new JButton(label);
                button.setFont(new Font(Font.DIALOG, Font.PLAIN, 46));
                button.addActionListener(this);
            }else{
                button = new JButton();
            }


            buttonpanel.add(button);
        }

        grid.setHgap(25);
        grid.setVgap(25);

        frame.add(buttonpanel);

        layout1.putConstraint(SpringLayout.NORTH, buttonpanel, 150, SpringLayout.NORTH, frame);
        layout1.putConstraint(SpringLayout.WEST, buttonpanel, 33, SpringLayout.WEST, frame);

        frame.setVisible(true);
    }


    public static void main(String[] args) {

        new Main();

    }

    @Override
    public void actionPerformed(ActionEvent e) {

        String buttonLabel = e.getActionCommand();

        if(buttonLabel.matches("[0-9]")){

            if(calcWindow.getText().equals("0") || signPressed){

                calcWindow.setText(buttonLabel);
                signPressed = false;

            }else{
                calcWindow.setText(calcWindow.getText() + buttonLabel);

            }


        } else if (buttonLabel.equals(".")) {

            if(signPressed){
                calcWindow.setText(buttonLabel);
                signPressed = false;

            }else if(!calcWindow.getText().contains(".")){
                calcWindow.setText(calcWindow.getText() + buttonLabel);
            }


        }else if (buttonLabel.equals("=")) {

            double number2 = Double.parseDouble(calcWindow.getText());

            double res;
            switch(sign){
                case "+":
                    res = number1 + number2;
                    break;

                case "-":
                    res = number1 - number2;
                    break;

                case "x":
                    res = number1 * number2;
                    break;

                case "/":

                    if(number2 != 0){
                        res = number1 / number2;
                    }else{
                        JFrame alert = new JFrame();
                        JLabel notice = new JLabel("Cholero, nie dziel przez 0!");
                        JPanel alertPanel = new JPanel();
                        alert.setSize(200, 100);
                        alert.setLocationRelativeTo(null);
                        alertPanel.add(notice);
                        alert.add(alertPanel);
                        alert.setVisible(true);
                        alert.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

                        frame.setVisible(false);
                        res = Double.parseDouble(calcWindow.getText());

                    }

                    break;

                default:
                    res = Double.parseDouble(calcWindow.getText());

            }

            calcWindow.setText(Double.toString(res));
            signPressed = false;
            sign = "0";

        }else if(buttonLabel.matches("[+x/-]")){
            number1 = Double.parseDouble(calcWindow.getText());
            sign = buttonLabel;
//            System.out.println(number1);
            signPressed = true;
        }else if(buttonLabel.equals("C")){
            number1 = 0;
            sign = "0";
            calcWindow.setText("0");
        } else if (buttonLabel.equals("<")) {

            if((calcWindow.getText().length() - 1) <= 0){
                calcWindow.setText("0");
            }else{
                calcWindow.setText(calcWindow.getText().substring(0, calcWindow.getText().length() - 1));
            }

        } else if (buttonLabel.equals("+/-")) {
            if(calcWindow.getText().contains("-")){
                calcWindow.setText(calcWindow.getText().substring(1));
            }else{
                calcWindow.setText("-" + calcWindow.getText());
            }

        }

    }
}