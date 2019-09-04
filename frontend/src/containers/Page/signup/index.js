import React, { Component } from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import signinImg from "../../../images/signin.svg";
import TextField from "../../../components/uielements/textfield";
import Scrollbars from "../../../components/utility/customScrollBar";
import Button from "../../../components/uielements/button";
import IntlMessages from "../../../components/utility/intlMessages";
import SignUpStyleWrapper from "./signup.style";
import { Checkbox } from "./signup.style";
import { authActions } from 'redux/actions';

class SignUp extends Component {
  state = {
    redirectToReferrer: false,
    params: {}
  };
  componentWillReceiveProps(nextProps) {
    if (
      this.props.isLoggedIn !== nextProps.isLoggedIn &&
      nextProps.isLoggedIn === true
    ) {
      this.setState({ redirectToReferrer: true });
    }
  }
  handleLogin = () => {
    const { login } = this.props;
    login();
    this.props.history.push("/dashboard");
  };
  render() {
    return (
      <SignUpStyleWrapper className="mateSignUpPage">
        <div className="mateSignInPageImgPart">
          <div className="mateSignInPageImg">
            <img src={signinImg} alt="Kiwi standing on oval" />
          </div>
        </div>

        <div className="mateSignInPageContent">
          <div className="mateSignInPageLink">
            <Link to="#">
              <button className="mateSignInPageLinkBtn active" type="button">
                Register
              </button>
            </Link>
            <Link to="/signin">
              <button className="mateSignInPageLinkBtn " type="button">
                Login
              </button>
            </Link>
          </div>
          <Scrollbars style={{ height: "100%" }}>
            <div className="mateSignInPageGreet">
              <h1>Its Free, Join Us</h1>
              <p>
                Welcome to ScanTX, Please SignUp with your account
                information.
              </p>
            </div>
            <div className="mateSignInPageForm">
              <div className="mateInputWrapper">
                <TextField
                  label="Username"
                  placeholder="Username"
                  margin="normal"
                />
              </div>
              <div className="mateInputWrapper">
                <TextField
                  label="Email"
                  placeholder="Email"
                  margin="normal"
                  type="Email"
                />
              </div>
              <div className="mateInputWrapper">
                <TextField
                  label="Password"
                  placeholder="Password"
                  margin="normal"
                  type="Password"
                />
              </div>
            </div>
            <div className="mateAgreement">
              <div className="mateLoginSubmitCheck">
                <Checkbox color="primary" className="mateTermsCheck" />
                <span className="mateTermsText">
                  <IntlMessages id="page.signUpContactByEmail" />
                </span>
              </div>
              <div className="mateLoginSubmitCheck">
                <Checkbox color="primary" className="mateTermsCheck" />
                <span className="mateTermsText">
                  <IntlMessages id="page.signEncryptionAction" />
                </span>
              </div>
              <div className="mateLoginSubmit">
                <Button type="primary" onClick={() => this.onRegister()}>
                  Sign Up
                </Button>
              </div>
            </div>
          </Scrollbars>
        </div>
      </SignUpStyleWrapper>
    );
  }
}
const mapSateToProps = state => {
  return {
      companies: state.Company.list,
      isLoggedIn: state.Auth.idToken !== null ? true : false
  };
};

const mapDispatchToProps = {
  register: authActions.register
};
export default connect(
  mapSateToProps,
  mapDispatchToProps
)(SignUp);
