import React, { Component } from "react";
import { Redirect } from "react-router-dom";
import { connect } from "react-redux";
import signinImg from "../../../images/signup.svg";
import Button from "../../../components/uielements/button";
import authAction from "../../../redux/auth/actions";
import TextField from "../../../components/uielements/textfield";
import Scrollbars from "../../../components/utility/customScrollBar";
import SignInStyleWrapper from "./signin.style";
import { CircularProgress } from "components/uielements/progress";
import { Link } from "react-router-dom";

const { login } = authAction;
class SignIn extends Component {
  state = {
    redirectToReferrer: false,
    username: "",
    password: "",
    loading: false,
    error: {
      status: false,
      message: ""
    }
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
    const { username, password } = this.state;
    this.setState(
      {
        loading: true
      },
      () => {
        login({ username, password }, this.onSuccess, this.onFail);
      }
    );
  };

  onSuccess = () => {
    this.setState(
      {
        loading: false
      },
      () => {
        this.props.history.push("/dashboard");
      }
    );
  };

  onFail = error => {
    this.setState({
      loading: false,
      error: {
        status: true,
        message: error
      }
    });
  };

  onChangeUsername = event => this.setState({ username: event.target.value });

  onChangePassword = event => this.setState({ password: event.target.value });

  render() {
    const from = { pathname: "/dashboard" };
    const { redirectToReferrer, username, password } = this.state;

    if (redirectToReferrer) {
      return <Redirect to={from} />;
    }
    return (
      <SignInStyleWrapper className="mateSignInPage">
        <div className="mateSignInPageImgPart">
          <div className="mateSignInPageImg">
            <img src={signinImg} alt="Kiwi standing on oval" />
          </div>
        </div>
        <div className="mateSignInPageContent">
          <Scrollbars style={{ height: "100%" }}>
            <div>
              {this.state.error.status && <h3>{this.state.error.message}</h3>}
            </div>
            <div>
              <div>
                <TextField
                  label="Username"
                  placeholder="Username"
                  margin="normal"
                  fullWidth
                  value={username}
                  onChange={this.onChangeUsername}
                />
              </div>
              <div className="mateInputWrapper">
                <TextField
                  label="Password"
                  placeholder="Password"
                  margin="normal"
                  fullWidth
                  type="Password"
                  value={password}
                  onChange={this.onChangePassword}
                />
              </div>
              {this.state.loading ? (
                <CircularProgress />
              ) : (
                <div className="mateLoginSubmit">
                  <Button
                    type="primary"
                    variant="contained"
                    color="primary"
                    margin="normal"
                    fullWidth
                    onClick={this.handleLogin}
                  >
                    Login
                  </Button>
                </div>
              )}
            </div>
            <Link to="/signup" variant="body2" margin="normal">
              Register
            </Link>
          </Scrollbars>
        </div>
      </SignInStyleWrapper>
    );
  }
}
export default connect(
  state => ({
    isLoggedIn: state.Auth.idToken !== null ? true : false
  }),
  { login }
)(SignIn);
